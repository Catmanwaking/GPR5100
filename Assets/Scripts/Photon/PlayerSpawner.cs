using UnityEngine;
using Photon.Pun;
using System;
using System.Collections;

[Serializable]
public class PlayerSpawner
{
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private float respawnTime;

    public GameObject SpawnPlayerOnServer()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        player.GetComponent<PlayerHealth>().OnDeath += RespawnPlayerDelay;
        return player;
    }

    public void RespawnPlayer(GameObject player)
    {
        int spawnIndex = UnityEngine.Random.Range(0, spawnPositions.Length);
        player.transform.position = spawnPositions[spawnIndex].position;
        player.transform.rotation = spawnPositions[spawnIndex].rotation;

        player.GetComponent<PlayerHealth>().ResetHealth();
        player.GetComponentInChildren<WeaponController>().ResetWeapon();
    }

    public void RespawnPlayerDelay(GameObject player)
    {
        GameManager.Instance.StartCoroutine(RespawnPlayerDelayRoutine(player));
    }

    private IEnumerator RespawnPlayerDelayRoutine(GameObject player)
    {
        GameManager.Instance.SetRespawnCam(true);
        RespawnPlayer(player);
        player.SetActive(false);
        yield return new WaitForSecondsRealtime(respawnTime);
        player.SetActive(true);
        GameManager.Instance.SetRespawnCam(false);
    }
}
