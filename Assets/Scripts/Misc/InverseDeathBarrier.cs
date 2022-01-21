using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseDeathBarrier : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("killing player");
        other.GetComponent<PlayerHealth>().TakeDamage(1000000.0f);
    }
}
