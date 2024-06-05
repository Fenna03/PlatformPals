using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuController : MonoBehaviour
{
    [SerializeField] private string VersionName = "0.1";
    [SerializeField] private GameObject usernameMenu;
    [SerializeField] private GameObject connectPanel;

    [SerializeField] private InputField usernameInput;
    [SerializeField] private InputField createGameInput;
    [SerializeField] private InputField joinGameInput;

    [SerializeField] private GameObject startButton;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(VersionName);
    }

    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("connected");
    }

    // Start is called before the first frame update
    private void Start()
    {
        usernameMenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeUsernameInput()
    {
        if (usernameInput.text.Length >= 3)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
    }

    public void setUserName()
    {
        usernameMenu.SetActive(false);
        PhotonNetwork.playerName = usernameInput.text;
    }

    public void createGame()
    {
        PhotonNetwork.CreateRoom(createGameInput.text, new RoomOptions() { MaxPlayers = 4}, null);

    }

    public void JoinGame()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(joinGameInput.text, roomOptions, TypedLobby.Default);
    }

    private void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("level1");
    }
}
