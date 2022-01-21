using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class RoomMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_Text hostName_Text;

    public void SetMasterClient(Player[] players)
    {
        foreach (Player player in players)
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
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(1); //MainMenuScene
    }
}
