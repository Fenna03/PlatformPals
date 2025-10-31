using UnityEngine;
using Unity.Netcode;

public class SceneDebug : MonoBehaviour
{
    void Start()
    {
        var netObjs = FindObjectsOfType<NetworkObject>();
        Debug.Log($"[SceneDebug] Found {netObjs.Length} NetworkObjects in scene.");
        foreach (var n in netObjs)
        {
            Debug.Log($"[SceneDebug] {n.name}  IsSpawned:{n.IsSpawned}  IsSceneObject:{n.IsSceneObject}  NetworkObjectId:{n.NetworkObjectId}  Owner:{n.OwnerClientId}");
        }
    }
}