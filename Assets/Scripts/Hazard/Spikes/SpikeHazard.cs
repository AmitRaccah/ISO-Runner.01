using System;
using UnityEngine;

public class SpikeHazard : MonoBehaviour
{
    public static event Action<SlowData> OnPlayerHitHazard;

    [SerializeField] private SlowData slowData;
    [SerializeField] private AudioClip hitSfx;
    [SerializeField] private float hitSfxVolume = 1f;

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HAZARD TRIGGER ENTER: " + other.name + " tag=" + other.tag, this);

        if (!other.CompareTag("Player"))
        {
            Debug.Log("NOT PLAYER - ignored", this);
            return;
        }

        AudioService audioService = AudioService.Instance;
        if (audioService != null)
        {
            audioService.PlaySfx(hitSfx, hitSfxVolume);
        }

        if (slowData == null)
        {
            Debug.LogWarning("SpikeHazard has no SlowData assigned.", this);
            return;
        }

        Debug.Log("PLAYER HIT HAZARD -> SEND EVENT", this);

        if (OnPlayerHitHazard != null)
        {
            OnPlayerHitHazard.Invoke(slowData);
        }
    }
}
