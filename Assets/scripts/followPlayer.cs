using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class followPlayer : NetworkBehaviour
{
    public CinemachineVirtualCamera vcam;
    private CinemachineTargetGroup targetGroup;
    public GameObject[] players;

    private void Start()
    {
        targetGroup = GameObject.Find("Target Group").GetComponent<CinemachineTargetGroup>();
        UpdatePlayers();
    }

    public void Update()
    {
        // Update players only if there is a change in the number of players
        if (players.Length != GameObject.FindGameObjectsWithTag("Player").Length)
        {
            UpdatePlayers();
        }
    }

    private void UpdatePlayers()
    {
        // Clear the target group and re-add all active players
        targetGroup.m_Targets = new CinemachineTargetGroup.Target[0];
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var player in players)
        {
            if (player != null) // Check if the player object is valid
            {
                var targetTransform = player.GetComponent<Transform>();
                if (targetTransform != null)
                {
                    targetGroup.AddMember(targetTransform, 1, 2); // Adding with weight=1 and radius=2
                }
            }
        }
    }
}
