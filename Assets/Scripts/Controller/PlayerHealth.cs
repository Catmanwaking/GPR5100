using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
    [SerializeField] private float maxHealth;
    private Hashtable playerHashTable;

    public System.Action<float> OnHealthChanged;
    public System.Action<GameObject> OnDeath;

    private bool isDead;

    private void Start()
    {
        ResetHealth();

        MethodLinker.Instance.LinkToHudHealth(ref OnHealthChanged);
    }

    public void ResetHealth()
    {
        isDead = false;
        if (!photonView.IsMine)
            return;

        playerHashTable = photonView.Controller.CustomProperties;
        playerHashTable["Health"] = maxHealth;
        photonView.Controller.SetCustomProperties(playerHashTable);
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
                Debug.Log("player died");
                int deaths = (int)playerHashTable["Deaths"];
                playerHashTable["Deaths"] = deaths++;
                playerHashTable["Health"] = 100.0f;
                photonView.Controller.SetCustomProperties(playerHashTable);
                OnDeath?.Invoke(this.gameObject);
            }
        }
    }
}