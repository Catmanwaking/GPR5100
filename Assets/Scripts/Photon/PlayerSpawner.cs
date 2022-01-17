using UnityEngine;
using Photon.Pun;
using System;

[Serializable]
public class PlayerSpawner
{
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private Transform[] spawnPositions;

    public GameObject SpawnPlayerOnServer()
    {
        return PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
    }

    public void RespawnPlayer(GameObject player)
    {
        int spawnIndex = UnityEngine.Random.Range(0, spawnPositions.Length);
        player.transform.position = spawnPositions[spawnIndex].position;
        player.transform.rotation = spawnPositions[spawnIndex].rotation;
        player.SetActive(true);
    }
}
