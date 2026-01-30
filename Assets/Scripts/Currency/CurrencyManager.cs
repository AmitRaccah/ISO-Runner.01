using System;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private Dictionary<CurrencyData, int> _inventory;

    // UI listens to this
    public static event Action<CurrencyData, int> OnCurrencyUpdated;

    private void Awake()
    {
        _inventory = new Dictionary<CurrencyData, int>();
    }

    private void OnEnable()
    {
        CollectibleBase.OnCollected += HandleItemCollected;
    }

    private void OnDisable()
    {
        CollectibleBase.OnCollected -= HandleItemCollected;
    }

    private void HandleItemCollected(CollectibleBase item)
    {
        CurrencyItem currencyItem;
        CurrencyData data;

        // 1) Make sure this collectible is a CurrencyItem
        currencyItem = item as CurrencyItem;
        if (currencyItem == null)
        {
            return;
        }

        // 2) Get the data
        data = currencyItem.data;
        if (data == null)
        {
            Debug.LogWarning("CurrencyItem has no CurrencyData assigned.");
            return;
        }

        // 3) Ensure key exists
        if (_inventory.ContainsKey(data) == false)
        {
            _inventory.Add(data, 0);
        }

        // 4) Add value
        _inventory[data] = _inventory[data] + data.value;

        // 5) Notify UI (send full data + new total)
        if (OnCurrencyUpdated != null)
        {
            OnCurrencyUpdated.Invoke(data, _inventory[data]);
        }

        Debug.Log("Collected " + data.currencyName + ". Total: " + _inventory[data]);
    }
}