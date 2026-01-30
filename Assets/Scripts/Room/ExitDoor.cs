using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private GameObject doorObject;

    private void OnEnable()
    {
        RoomManager.OnRoomCompleted += HandleRoomCompleted;
    }

    private void OnDisable()
    {
        RoomManager.OnRoomCompleted -= HandleRoomCompleted;
    }

    private void HandleRoomCompleted()
    {
        if (doorObject != null)
        {
            doorObject.SetActive(false);
        }
    }
}