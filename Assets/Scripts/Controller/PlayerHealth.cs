using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
    [SerializeField] private float maxHealth;
    private Hashtable playerHashTable;

    public System.Action<float> OnHealthChanged;

    private void Start()
    {
        if (!photonView.IsMine)
            return;

        playerHashTable = new Hashtable();
        playerHashTable.Add("health", maxHealth);

        MethodLinker.Instance.LinkToHudHealth(ref OnHealthChanged);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerHashTable);
    }

    public void TakeDamage(float damage)
    {
        float health = (float)playerHashTable["health"];

        health -= damage;
        if (health <= 0.0f)
            health = 0.0f;

        OnHealthChanged?.Invoke(health);
        playerHashTable["health"] = health;
        photonView.Controller.SetCustomProperties(playerHashTable);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (photonView.Controller == targetPlayer)
        {
            playerHashTable = changedProps;
        }
    }
}