using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MethodLinker : MonoBehaviour
{
    public static MethodLinker Instance;
    //please teach us how to not do this

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;
    }

    [SerializeField] private HUDManager hud;

    public void LinkToHudAmmo(ref System.Action<int> action)
    {
        action += hud.SetAmmo;
    }

    public void LinkToHudHealth(ref System.Action<float> action)
    {
        action += hud.SetHealth;
    }
}
