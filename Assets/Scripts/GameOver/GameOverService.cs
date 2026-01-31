using UnityEngine;

public class GameOverService : MonoBehaviour
{
    [SerializeField] private GameOverUI gameOverUI;

    [Header("Messages")]
    [SerializeField] private string caughtMessage = "GAME OVER\nהמפלצת תפסה אותך!";
    [SerializeField] private string winMessage = "כל הכבוד!\nהגעת לסוף!";

    private bool isGameOver;

    private void Awake()
    {
        if (gameOverUI == null)
        {
            gameOverUI = FindFirstObjectByType<GameOverUI>();
        }
    }

    private void OnEnable()
    {
        MonsterCatchTrigger.OnPlayerCaught += HandlePlayerCaught;
        FinishTrigger.OnPlayerReachedFinish += HandlePlayerReachedFinish;
    }

    private void OnDisable()
    {
        MonsterCatchTrigger.OnPlayerCaught -= HandlePlayerCaught;
        FinishTrigger.OnPlayerReachedFinish -= HandlePlayerReachedFinish;
    }

    private void HandlePlayerCaught()
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;

        if (gameOverUI != null)
        {
            gameOverUI.Show(caughtMessage);
        }
    }

    private void HandlePlayerReachedFinish()
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;

        if (gameOverUI != null)
        {
            gameOverUI.Show(winMessage);
        }
    }
}