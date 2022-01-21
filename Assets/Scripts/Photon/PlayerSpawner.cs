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
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        player.GetComponent<PlayerHealth>().OnDeath += RespawnPlayer;
        return player;
    }

    public void RespawnPlayer(GameObject player)
    {
        int spawnIndex = UnityEngine.Random.Range(0, spawnPositions.Length);
        player.transform.position = spawnPositions[spawnIndex].position;
        player.transform.rotation = spawnPositions[spawnIndex].rotation;

        player.GetComponent<PlayerHealth>().ResetHealth();
        player.GetComponentInChildren<WeaponController>().ResetWeapon();

        player.SetActive(true);
    }
}
