using System;
using UnityEngine;

public class FadeZone : MonoBehaviour
{
    public static event Action<FadeData> OnFadeRequested;

    [SerializeField] private FadeData fadeData;
    [SerializeField] private bool disableOnTrigger = true;

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (fadeData == null)
        {
            Debug.LogWarning("FadeZone has no FadeData assigned.", this);
            return;
        }

        if (OnFadeRequested != null)
        {
            OnFadeRequested.Invoke(fadeData);
        }

        if (disableOnTrigger)
        {
            gameObject.SetActive(false);
        }
    }
}