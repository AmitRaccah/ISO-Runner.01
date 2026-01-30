using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private int currentTotal;

    private void Awake()
    {
        UpdateText();
    }

    private void OnEnable()
    {
        CurrencyManager.OnCurrencyUpdated += HandleCurrencyUpdated;
    }

    private void OnDisable()
    {
        CurrencyManager.OnCurrencyUpdated -= HandleCurrencyUpdated;
    }

    private void HandleCurrencyUpdated(CurrencyData data, int newTotal)
    {
        currentTotal = newTotal;
        UpdateText();
    }

    private void UpdateText()
    {
        if (scoreText == null)
        {
            return;
        }

        scoreText.text = currentTotal.ToString();
    }
}