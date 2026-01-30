using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public abstract class CollectibleBase : MonoBehaviour
{
    public static event Action<CollectibleBase> OnCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    public virtual void Collect()
    {
        OnCollected?.Invoke(this);
        
        gameObject.SetActive(false);
    }
}