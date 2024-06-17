using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class multiplayerGameLobby : MonoBehaviour
{
    public static multiplayerGameLobby Instance { get; private set; }

    private const string KEY_RELAY_JOIN_CODE = "RelayJoinCode";

    private Lobby joinedLobby;

    private float heartbeatTimer;
    private float listLobbiesTimer;

    public event EventHandler onCreateLobbyStarted;
    public event EventHandler onCreateLobbyFailed;
    public event EventHandler onJoinStarted;
    public event EventHandler onQuickJoinFailed;
    public event EventHandler onJoinFailed;
    public event EventHandler <onLobbyListChangedEventArgs> onLobbyListChanged;

    public class onLobbyListChangedEventArgs : EventArgs
    {
        public List<Lobby> lobbyList;
    }

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);

        InitializeUnityAuthentication();
    }

    private async void InitializeUnityAuthentication()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions options = new InitializationOptions();
            options.SetProfile(UnityEngine.Random.Range(0, 10000).ToString());
            await UnityServices.InitializeAsync(options);

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    private void Update()
    {
        handleHeartBeat();
        handlePeriodicListLobbies();
    }
    
    private void handlePeriodicListLobbies()
    {
        if (joinedLobby == null && AuthenticationService.Instance.IsSignedIn && SceneManager.GetActiveScene().name == Loader.Scene.Menu.ToString())
        {
            listLobbiesTimer -= Time.deltaTime;
            if (listLobbiesTimer <= 0f)
            {
                float listLobbiesTimerMax = 3f;
                listLobbiesTimer = listLobbiesTimerMax;
                listLobbies();
            }
        }
    }

    private void handleHeartBeat()
    {
        if (isLobbyHost())
        {
            heartbeatTimer -= Time.deltaTime;
            if( heartbeatTimer <= 0f )
            {
                float heartbeatTimerMax = 15f;
                heartbeatTimer = heartbeatTimerMax;

                LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }

    private bool isLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    private async void listLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions()
            {
                Filters = new List<QueryFilter>
            {
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
            }
            };

            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            onLobbyListChanged?.Invoke(this, new onLobbyListChangedEventArgs
            {
                lobbyList = queryResponse.Results
            });
        }catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async Task<Allocation> allocateRelay()
    {
        try
        {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(optionsScript.MAX_PLAYER_AMOUNT - 1);

            return allocation;
        }catch (RelayServiceException e) 
        { 
            Debug.Log(e);
            return default;
        }
    }

    private async Task<string> GetRelayJoinCode(Allocation allocation)
    {
        try
        {
            string relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            return relayJoinCode;
        }catch(RelayServiceException e)
        {
            Debug.Log(e);
            return default;
        }
    }

    private async Task<JoinAllocation> joinRelay(string joinCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            return joinAllocation;
        }catch(RelayServiceException e)
        {
            Debug.Log(e);
            return default;
        }
    }

    public async void createLobby(string lobbyName, bool isPrivate)
    {
        onCreateLobbyStarted?.Invoke(this, EventArgs.Empty);
        try
        {
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, optionsScript.MAX_PLAYER_AMOUNT, new CreateLobbyOptions
            {
                IsPrivate = isPrivate,
            });

            Allocation allocation = await allocateRelay();

            string relayJoinCode = await GetRelayJoinCode(allocation);

            await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    {KEY_RELAY_JOIN_CODE, 
                        new DataObject(DataObject.VisibilityOptions.Member,
                        relayJoinCode)}
                }
            });
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));

            //Debug.Log("start Host");
            optionsScript.Instance.startHost();
            Loader.loadNetwork(Loader.Scene.characterSelect);
        } catch (LobbyServiceException e)
        {
            Debug.Log(e);
            onCreateLobbyFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    public async void quickJoin()
    {
        onJoinStarted?.Invoke(this, EventArgs.Empty);
        try
        {
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();

            string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;

            JoinAllocation joinAllocation = await joinRelay(relayJoinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
            
            optionsScript.Instance.startClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            onQuickJoinFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    public async void joinWithId(string lobbyId)
    {
        onJoinStarted?.Invoke(this, EventArgs.Empty);
        try
        {
            joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);

            string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;

            JoinAllocation joinAllocation = await joinRelay(relayJoinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

            optionsScript.Instance.startClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            onJoinFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    public async void joinWithCode(string lobbyCode)
    {
        onJoinStarted?.Invoke(this, EventArgs.Empty);
        try
        {
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);

            string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;

            JoinAllocation joinAllocation = await joinRelay(relayJoinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

            optionsScript.Instance.startClient();
        }catch (LobbyServiceException e)
        {
            Debug.Log(e);
            onJoinFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    public async void deleteLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
                joinedLobby = null;
            } catch(LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public async void leaveLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
                
                joinedLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public async void KickPlayer(string playerId)
    {
        if (isLobbyHost())
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
                
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public Lobby GetLobby()
    {
        return joinedLobby;
    }
}
