using System;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    public static event Action OnGameStarted;

    [Header("State (read-only)")]
    [SerializeField] private bool isGameStarted;

    public bool IsGameStarted
    {
        get { return isGameStarted; }
    }

    public void StartGame()
    {
        if (isGameStarted)
        {
            return;
        }

        isGameStarted = true;

        if (OnGameStarted != null)
        {
            OnGameStarted.Invoke();
        }
    }
}