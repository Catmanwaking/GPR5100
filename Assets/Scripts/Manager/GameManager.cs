using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerSpawner spawner;
    [SerializeField] private GameObject demoCameraGO;
    [SerializeField] private GameObject hudGO;
    [SerializeField] private GameObject menuGO;
    [SerializeField] private GameObject readyGO;
    [SerializeField] private GameObject scoreGO;
    [SerializeField] private Scoreboard scoreboard;
    [SerializeField] private RoomMenuUI roomMenu;

    private GameObject localPlayerGO;
    private Player[] players;
    private int readyPlayerCount;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        players = PhotonNetwork.PlayerList;
        scoreboard.UpdateScoreboard(players);
        roomMenu.SetMasterClient(players);
        localPlayerGO = spawner.SpawnPlayerOnServer();
        localPlayerGO.SetActive(false);
    }

    private IEnumerator RoundTimer()
    {
        yield return new WaitForSecondsRealtime(3.0f * 60.0f);
        photonView.RPC(nameof(StopRoundRpc), RpcTarget.All);
        PhotonNetwork.CurrentRoom.IsOpen = true;
    }

    public void SetReady(bool isReady)
    {
        readyGO.SetActive(isReady);
        menuGO.SetActive(!isReady);
        scoreGO.SetActive(!isReady);
        photonView.RPC(nameof(SetReadyRpc), RpcTarget.MasterClient, isReady);
    }

    public void DisplayScore(bool display)
    {
        scoreGO.SetActive(display);
    }

    public void SetRespawnCam(bool isRespawning)
    {
        demoCameraGO.SetActive(isRespawning);
        hudGO.SetActive(!isRespawning);
    }
    
    public void SetActiveOnServer(int viewID, bool active)
    {
        photonView.RPC(nameof(SetActiveOnServerRpc), RpcTarget.Others, viewID, active);
    }

    [PunRPC]
    public void SetActiveOnServerRpc(int viewID, bool active)
    {
        PhotonView.Find(viewID).gameObject.SetActive(active);
    }

    [PunRPC]
    public void SetReadyRpc(bool isReady)
    {
        if (isReady) //problematic when player leaves during ready phase
            readyPlayerCount++;
        else
            readyPlayerCount--;

        if(readyPlayerCount == players.Length)
        {
            photonView.RPC(nameof(StartRoundRpc), RpcTarget.All);
            PhotonNetwork.CurrentRoom.IsOpen = false;
            StartCoroutine(RoundTimer());
        }
    }

    [PunRPC]
    public void StartRoundRpc()
    {
        spawner.RespawnPlayer(localPlayerGO);
        localPlayerGO.SetActive(true);

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
        scoreGO.SetActive(true);
        demoCameraGO.SetActive(true);
        menuGO.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        readyPlayerCount = 0;
    }



    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        players = PhotonNetwork.PlayerList;
        scoreboard.UpdateScoreboard(players);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        players = PhotonNetwork.PlayerList;
        scoreboard.UpdateScoreboard(players);
        roomMenu.SetMasterClient(players);
    }
}
