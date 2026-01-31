using UnityEngine;

public class StartGameTrigger : MonoBehaviour
{
    [SerializeField] private GameStartManager gameStartManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (gameStartManager == null)
        {
            return;
        }

        gameStartManager.StartGame();

        gameObject.SetActive(false);
    }
}