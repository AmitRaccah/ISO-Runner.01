using System;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    public static event Action OnPlayerReachedFinish;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (OnPlayerReachedFinish != null)
        {
            OnPlayerReachedFinish.Invoke();
        }
    }
}