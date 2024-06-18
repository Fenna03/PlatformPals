using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class followPlayer : NetworkBehaviour
{
    Cinemachine.CinemachineTargetGroup.Target target;
    public CinemachineVirtualCamera vcam;

    private CinemachineTargetGroup targetGroup;
    private GameObject[] players;
    private int playersConnected;


    private void Start()
    {
        targetGroup = GameObject.Find("Target Group").GetComponent<CinemachineTargetGroup>();
        players = GameObject.FindGameObjectsWithTag("Player");
        playersConnected = players.Length;
        target.weight = 1;
        target.radius = 2;
    }

    public void Update()    
    {
        for (int i = 0; i < players.Length; i++)
        {
            target.target = players[i].GetComponent<Transform>();

            targetGroup.AddMember(target.target, target.weight, target.radius);
        }
        if (players.Length <= 0)
        {
            Debug.Log("No players found");
        }

        if(playersConnected != GameObject.FindGameObjectsWithTag("Player").Length) 
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; i++)
            {
                target.target = players[i].GetComponent<Transform>();

                targetGroup.AddMember(target.target, target.weight, target.radius);
            }
        }
    }
}