using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class CollectibleBase : MonoBehaviour
{
    [SerializeField] private bool disableOnCollect = true;

    private void Reset()
    {
        // Ensures collider is trigger
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Let the derived class decide what to do (currency/key/etc)
        OnCollectedBy(other.gameObject);

        // Disable / pool
        if (disableOnCollect)
            gameObject.SetActive(false);
    }

    protected abstract void OnCollectedBy(GameObject collector);
}