using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
    [SerializeField] private float maxHealth;
    private Hashtable playerHashTable;

    public System.Action<float> OnHealthChanged;
    public System.Action<GameObject> OnDeath;

    private float currentHealth;

    private void Start()
    {
        ResetHealth();

        MethodLinker.Instance.LinkToHudHealth(ref OnHealthChanged);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(float damage)
    {
        photonView.RPC(nameof(TakeDamageRpc), photonView.Controller, damage);
    }

    [PunRPC]
    public void TakeDamageRpc(float damage)
    { 
        currentHealth -= damage;
        if (currentHealth <= 0.0f)
        {
            currentHealth = 0.0f;
            OnDeath?.Invoke(this.gameObject);
        }
        OnHealthChanged?.Invoke(currentHealth);
    }
}