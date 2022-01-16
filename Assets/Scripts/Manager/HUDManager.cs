using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TMP_Text AmmoText;
    [SerializeField] private Slider healthSlider;
    private float healthValue = 100;
    private int ammoValue = 30;

    private void Start()
    {
        UpdateAmmoText();
        UpdateHealthSlider();
    }

    public void SetAmmo(int newAmmo)
    {
        ammoValue = newAmmo;
        UpdateAmmoText();
    }

    public void SetHealth(float newHealth)
    {
        healthValue = newHealth;
        UpdateHealthSlider();
    }

    private void UpdateAmmoText()
    {
        AmmoText.text = $"Ammo: {ammoValue}";
    }

    private void UpdateHealthSlider()
    {
        healthSlider.value = healthValue;
    }
}
