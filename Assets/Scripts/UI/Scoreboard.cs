using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Scoreboard : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text[] Usernames_Text;
    [SerializeField] private TMP_Text[] Scores_Text;

    public void Initialize()
    {
        Hashtable customProps = new Hashtable
        {
            { "Health", 0.0f },
            { "Kills", 0 },
            { "Deaths", 0 }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(customProps);
    }

    public void UpdateScoreboard(Player[] players)
    {
        int playerCount = players.Length;
        for (int i = 0; i < playerCount; i++)
        {
            Player player = players[i];
            Usernames_Text[i].text = player.NickName;
            //Hashtable playerProps = player.CustomProperties;
            //Scores_Text[i].text = CreateScoreString(playerProps);
        }
        for (int i = players.Length; i < Usernames_Text.Length; i++)
        {
            Usernames_Text[i].text = string.Empty;
            Scores_Text[i].text = string.Empty;
        }
    }

    private string CreateScoreString(Hashtable playerProps)
    {
        int kills = (int)playerProps["Kills"];
        int deaths = (int)playerProps["Deaths"];
        string kdString = $"{kills,3}/{deaths,-3}";
        return kdString;
    }
}
