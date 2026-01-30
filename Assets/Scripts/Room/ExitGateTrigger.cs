using System;
using UnityEngine;

public class ExitGateTrigger : MonoBehaviour
{
    public static event Action OnPlayerPassedExitGate;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (OnPlayerPassedExitGate != null)
        {
            OnPlayerPassedExitGate.Invoke();
        }
    }
}