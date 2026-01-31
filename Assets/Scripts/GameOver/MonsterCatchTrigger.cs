using System;
using UnityEngine;

public class MonsterCatchTrigger : MonoBehaviour
{
    public static event Action OnPlayerCaught;

    [Header("Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool disableAfterCatch = true;

    private bool didCatch;

    private void OnTriggerEnter(Collider other)
    {
        if (didCatch)
        {
            return;
        }

        if (other == null)
        {
            return;
        }

        if (!other.CompareTag(playerTag))
        {
            return;
        }

        didCatch = true;

        if (OnPlayerCaught != null)
        {
            OnPlayerCaught.Invoke();
        }

        if (disableAfterCatch)
        {
            gameObject.SetActive(false);
        }
    }
}