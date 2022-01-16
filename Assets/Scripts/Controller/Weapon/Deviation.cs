//Author: Dominik Dohmeier

using System;
using UnityEngine;

[Serializable]
public class Deviation
{
    [SerializeField] private float minDeviation;
    [SerializeField] private float maxDeviation;
    [SerializeField] private float deviationIncreasePerShot;
    [SerializeField] private float deviationDecreasePerSecond;

    private float currentDeviation;
    private static Vector2 crosshairScalar;
    private static Vector2 crosshairBaseDistance;
    private static RectTransform crosshairRectTransform;

    /// <summary>
    /// Attaches and calculates values for accurate crosshair spread displaying.
    /// </summary>
    /// <param name="crosshair"> The Crosshair to display. </param>
    public static void AttachCrosshair(RectTransform crosshair)
    {
        if (crosshair == null)
            return;
        crosshairRectTransform = crosshair;
        crosshairScalar = Vector2.one / Mathf.Tan(Camera.main.fieldOfView * Mathf.Deg2Rad / 2.0f) * Screen.height;
        crosshairBaseDistance = crosshairRectTransform.sizeDelta;
    }

    /// <summary>
    /// Updates the deviation. Use this in Update.
    /// </summary>
    public void UpdateDeviation()
    {
        currentDeviation = Mathf.Clamp(currentDeviation - Time.deltaTime * deviationDecreasePerSecond, minDeviation, maxDeviation);
        if(crosshairRectTransform)
            crosshairRectTransform.sizeDelta = (crosshairScalar * currentDeviation) + crosshairBaseDistance;
    }

    /// <summary>
    /// Increases the deviation. Use this after every shot taken.
    /// </summary>
    public void IncreaseDeviation()
    {
        currentDeviation = Mathf.Clamp(currentDeviation + deviationIncreasePerShot, minDeviation, maxDeviation);
    }

    public void ResetDeviation()
    {
        currentDeviation = minDeviation;
    }

    public Vector3 GetRandomSpreadVector()
    {
        Vector3 shotDirection = UnityEngine.Random.insideUnitCircle * currentDeviation;
        shotDirection.z = 1.0f;
        return shotDirection;
    }

    public static implicit operator float(Deviation deviation) => deviation.currentDeviation;
}

