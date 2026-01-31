using UnityEngine;

[CreateAssetMenu(fileName = "NewCurrency", menuName = "Runner/CurrencyData")]
public class CurrencyData : ScriptableObject
{
    public string currencyName;
    public int value;
    public AudioClip collectSfx;
    public float collectSfxVolume = 1f;
    //GIZMOS
    public Color debugColor = Color.yellow;
}
