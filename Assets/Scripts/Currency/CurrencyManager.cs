using System;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private Dictionary<CurrencyData, int> _inventory = new();

    public static event Action<CurrencyData, int> OnCurrencyUpdated;

    private void OnEnable()
    {
        CurrencyItem.OnCurrencyCollected += HandleCurrencyCollected;
    }

    private void OnDisable()
    {
        CurrencyItem.OnCurrencyCollected -= HandleCurrencyCollected;
    }

    private void HandleCurrencyCollected(CurrencyData data)
    {
        if (_inventory.ContainsKey(data) == false)
            _inventory[data] = 0;

        _inventory[data] += data.value;

        OnCurrencyUpdated?.Invoke(data, _inventory[data]);
        Debug.Log($"Collected {data.currencyName}. Total: {_inventory[data]}");
    }
}