using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TMP_Text ammo_Text;
    [SerializeField] private Slider health_Slider;
    [SerializeField] private RectTransform crosshair_RectTransform;
    private float healthValue = 100;
    private int ammoValue = 30;
    private float deviationValue;

    private Vector2 crosshairScalar;
    private Vector2 crosshairBaseDistance;

    private void Awake()
    {
        CalculateCrosshairValues(90.0f); //TODO MAGIC NUMBER... GET FOV OF LOCAL PLAYER
    }

    private void Start()
    {       
        UpdateAmmoText();
        UpdateHealthSlider();
        UpdateCrosshair();
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

    public void SetDeviation(float newDeviation)
    {
        deviationValue = newDeviation;
        UpdateCrosshair();
    }

    private void UpdateAmmoText()
    {
        ammo_Text.text = $"Ammo: {ammoValue}";
    }

    private void UpdateHealthSlider()
    {
        health_Slider.value = healthValue;
    }

    private void UpdateCrosshair()
    {
        crosshair_RectTransform.sizeDelta = (crosshairScalar * deviationValue) + crosshairBaseDistance;
    }

    private void CalculateCrosshairValues(float FOV)
    {
        crosshairScalar = Vector2.one / Mathf.Tan(FOV * Mathf.Deg2Rad / 2.0f) * Screen.height;
        crosshairBaseDistance = crosshair_RectTransform.sizeDelta;
    }
}
