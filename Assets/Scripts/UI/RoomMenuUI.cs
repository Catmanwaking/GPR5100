using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine.SceneManagement;

public class RoomMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_Text hostName_Text;
    [SerializeField] private TMP_Text roomName_Text;
    [SerializeField] private GameObject start_ButtonGO;

    // Start is called before the first frame update
    void Start()
    {
        roomName_Text.text = PhotonNetwork.CurrentRoom.Name;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.IsMasterClient)
                hostName_Text.text = player.NickName;
        }
    }

    public void StartGame()
    {
        GameManager.Instance.StartRoundRpc();
    }

    public void ReturnToMenu()
    {
        StartCoroutine(Disconnect());
    }

    private IEnumerator Disconnect()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
        SceneManager.LoadScene(1); //MainMenuScene
    }
}
