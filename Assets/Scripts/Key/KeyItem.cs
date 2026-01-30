using System;
using UnityEngine;

public class KeyItem : CollectibleBase
{
    public static event Action<KeyData> OnKeyCollected;

    [SerializeField] private KeyData key;

    protected override void OnCollectedBy(GameObject collector)
    {
        if (key == null)
        {
            Debug.LogWarning("KeyItem has no KeyData assigned.", this);
            return;
        }

        OnKeyCollected?.Invoke(key);
    }
}