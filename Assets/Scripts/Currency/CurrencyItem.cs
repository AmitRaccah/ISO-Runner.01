using System;
using UnityEngine;

public class CurrencyItem : CollectibleBase
{
    public static event Action<CurrencyData> OnCurrencyCollected;

    [SerializeField] private CurrencyData data;

    protected override void OnCollectedBy(GameObject collector)
    {
        if (data == null)
        {
            Debug.LogWarning("CurrencyItem has no CurrencyData assigned.", this);
            return;
        }

        OnCurrencyCollected?.Invoke(data);

        AudioService audioService = AudioService.Instance;
        if (audioService != null)
        {
            audioService.PlaySfx(data.collectSfx, data.collectSfxVolume);
        }
    }
}
