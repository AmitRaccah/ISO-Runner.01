using System;
using UnityEngine;

public class KeyItem : CollectibleBase
{
    public static event Action OnKeyCollected;

    protected override void OnCollectedBy(GameObject collector)
    {
        if (OnKeyCollected != null)
        {
            OnKeyCollected.Invoke();
        }
    }
}