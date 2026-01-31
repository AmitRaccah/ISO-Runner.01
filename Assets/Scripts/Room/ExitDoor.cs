using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject doorObject;

    [Tooltip("RoomManager in the scene (the one that runs rooms).")]
    [SerializeField] private RoomManager roomManager;

    [Tooltip("Which RoomData this door belongs to.")]
    [SerializeField] private RoomData doorRoom;

    private void Awake()
    {
        if (doorObject == null)
        {
            Debug.LogWarning("ExitDoor: doorObject is NULL.", this);
        }

        if (roomManager == null)
        {
            roomManager = FindFirstObjectByType<RoomManager>();
        }

        if (roomManager == null)
        {
            Debug.LogWarning("ExitDoor: roomManager is NULL (not found). Assign it in Inspector.", this);
        }

        if (doorRoom == null)
        {
            Debug.LogWarning("ExitDoor: doorRoom is NULL. Assign RoomData for this door.", this);
        }
    }

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
        if (doorObject == null || roomManager == null || doorRoom == null)
        {
            return;
        }

        // Only open if THIS door belongs to the room that was completed (current active room)
        if (roomManager.CurrentRoom != doorRoom)
        {
            return;
        }

        doorObject.SetActive(false);
    }
}