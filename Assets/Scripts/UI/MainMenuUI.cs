using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName = "YOUR_GAMEPLAY_SCENE";

    public void OnStartClicked()
    {
        if (string.IsNullOrEmpty(gameplaySceneName))
        {
            return;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(gameplaySceneName);
    }
}