using UnityEngine;

public class RoomStarter : MonoBehaviour
{
    [SerializeField] private RoomData room;
    [SerializeField] private RoomManager roomManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (roomManager == null)
        {
            return;
        }

        roomManager.StartRoom(room);

        gameObject.SetActive(false);
    }
}