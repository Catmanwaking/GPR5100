using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
    [SerializeField] private float maxHealth;
    private Hashtable playerHashTable;

    public System.Action<float> OnHealthChanged;
    public System.Action OnDeath;

    private bool isDead;

    private void Start()
    {
        isDead = false;
        if (!photonView.IsMine)
            return;

        playerHashTable = photonView.Controller.CustomProperties;
        playerHashTable["Health"] = maxHealth;
        photonView.Controller.SetCustomProperties(playerHashTable);

        MethodLinker.Instance.LinkToHudHealth(ref OnHealthChanged);
    }

    public bool TakeDamage(float damage)
    {
        if (isDead)
            return false;
        bool confirmedKill = false;
        float health = (float)playerHashTable["Health"];

        health -= damage;
        if (health <= 0.0f)
        {
            health = 0.0f;
            confirmedKill = true;
            isDead = true;
        }

        playerHashTable = photonView.Controller.CustomProperties; //TODO check if needed
        playerHashTable["Health"] = health;
        photonView.Controller.SetCustomProperties(playerHashTable);

        return confirmedKill;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (photonView.Controller == targetPlayer)
        {
            playerHashTable = changedProps;
            float health = (float)playerHashTable["Health"];
            OnHealthChanged?.Invoke(health);
            //TODO damage sound, death check 
            if(health <= 0.0f)
            {
                int deaths = (int)playerHashTable["Deaths"];
                playerHashTable["Deaths"] = deaths++;
                photonView.Controller.SetCustomProperties(playerHashTable);
                OnDeath?.Invoke();
            }
        }
    }
}