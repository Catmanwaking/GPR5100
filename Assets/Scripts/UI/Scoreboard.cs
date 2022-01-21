using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Scoreboard : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text[] Usernames_Text;
    [SerializeField] private TMP_Text[] Scores_Text;

    private Player[] players;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        players = PhotonNetwork.PlayerList;

        Hashtable customProps = new Hashtable
        {
            { "Health", 0.0f },
            { "Kills", 0 },
            { "Deaths", 0 }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(customProps);
    }

    public void UpdateScoreboard()
    {
        int playerCount = players.Length;
        for (int i = 0; i < playerCount; i++)
        {
            Player player = players[i];
            Hashtable playerProps = player.CustomProperties;
            Usernames_Text[i].text = player.NickName;
            Scores_Text[i].text = CreateScoreString(playerProps);
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

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        targetPlayer.CustomProperties = changedProps; //this maybe?
        UpdateScoreboard();
    }
}
