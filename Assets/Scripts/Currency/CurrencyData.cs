using UnityEngine;

[CreateAssetMenu(fileName = "NewCurrency", menuName = "Runner/CurrencyData")]
public class CurrencyData : ScriptableObject
{
    public string currencyName;
    public int value;
    //GIZMOS
    public Color debugColor = Color.yellow;
}