using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Scoreboard : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text[] Usernames_Text;
    [SerializeField] private TMP_Text[] Scores_Text;

    public void UpdateScoreboard(Player[] players)
    {
        int playerCount = players.Length;
        for (int i = 0; i < playerCount; i++)
        {
            Player player = players[i];
            Usernames_Text[i].text = player.NickName;
        }
        for (int i = players.Length; i < Usernames_Text.Length; i++)
        {
            Usernames_Text[i].text = string.Empty;
            Scores_Text[i].text = string.Empty;
        }
    }
}
