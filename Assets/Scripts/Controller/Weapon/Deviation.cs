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

    public Action<float> OnDeviationChange;

    /// <summary>
    /// Updates the deviation. Use this in Update.
    /// </summary>
    public void UpdateDeviation()
    {
        float newDeviation = Mathf.Clamp(currentDeviation - Time.deltaTime * deviationDecreasePerSecond, minDeviation, maxDeviation);
        if(currentDeviation != newDeviation)
        {
            currentDeviation = newDeviation;
            OnDeviationChange?.Invoke(currentDeviation);
        }
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
        OnDeviationChange?.Invoke(currentDeviation);
    }

    public Vector3 GetRandomSpreadVector()
    {
        Vector3 shotDirection = UnityEngine.Random.insideUnitCircle * currentDeviation;
        shotDirection.z = 1.0f;
        return shotDirection;
    }

    public static implicit operator float(Deviation deviation) => deviation.currentDeviation;
}

