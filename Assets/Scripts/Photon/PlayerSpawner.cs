using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private Transform[] spawnPositions;

    private void Start()
    {
        //spawnPosition logic
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[0].position, spawnPositions[0].rotation);
    }
}
