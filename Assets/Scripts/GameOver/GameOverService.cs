using UnityEngine;

public class GameOverService : MonoBehaviour
{
    [SerializeField] private GameOverUI gameOverUI;

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
        MonsterCatchTrigger.OnPlayerCaught += HandleGameOver;
        FinishTrigger.OnPlayerReachedFinish += HandleGameOver;
    }

    private void OnDisable()
    {
        MonsterCatchTrigger.OnPlayerCaught -= HandleGameOver;
        FinishTrigger.OnPlayerReachedFinish -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;

        if (gameOverUI != null)
        {
            gameOverUI.Show();
        }
    }
}