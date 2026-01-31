using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panelRoot;

    [Header("Main Menu")]
    [SerializeField] private bool enableMainMenuButton = false;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool isShown;

    private void Awake()
    {
        Hide();
    }

    public void Show()
    {
        isShown = true;

        if (panelRoot != null)
        {
            panelRoot.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void Hide()
    {
        isShown = false;

        Time.timeScale = 1f;

        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
        }
    }

    public void OnRestartClicked()
    {
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

    public void OnMainMenuClicked()
    {
        if (enableMainMenuButton == false)
        {
            return;
        }

        if (string.IsNullOrEmpty(mainMenuSceneName))
        {
            return;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public bool IsShown()
    {
        return isShown;
    }
}