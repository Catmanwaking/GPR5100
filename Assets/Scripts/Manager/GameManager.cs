using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerSpawner spawner;
    [SerializeField] private GameObject demoCameraGO;
    [SerializeField] private GameObject hudGO;
    [SerializeField] private GameObject menuGO;
    [SerializeField] private GameObject readyGO;

    private GameObject localPlayerGO;
    private int readyPlayerCount;
    private Player[] players;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        players = PhotonNetwork.PlayerList;
        localPlayerGO = spawner.SpawnPlayerOnServer();
        localPlayerGO.SetActive(false);
    }

    public void SetReady(bool isReady)
    {
        readyGO.SetActive(isReady);
        menuGO.SetActive(!isReady);
        photonView.RPC(nameof(SetReadyRpc), RpcTarget.MasterClient, isReady);
    }

    [PunRPC]
    public void SetReadyRpc(bool isReady)
    {
        if (isReady)
            readyPlayerCount++;
        else
            readyPlayerCount--;

        if(readyPlayerCount == players.Length)
            photonView.RPC(nameof(StartRoundRpc), RpcTarget.All);
        //TODO stop round
    }

    [PunRPC]
    public void StartRoundRpc()
    {
        spawner.RespawnPlayer(localPlayerGO);
        hudGO.SetActive(true);
        localPlayerGO.SetActive(true);
        demoCameraGO.SetActive(false);
        readyGO.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    [PunRPC]
    public void StopRoundRpc()
    {
        hudGO.SetActive(false);
        localPlayerGO.SetActive(false);
        demoCameraGO.SetActive(true);
        menuGO.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        players = PhotonNetwork.PlayerList;
    }
    
    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        players = PhotonNetwork.PlayerList;
    }
}
