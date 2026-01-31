using System;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static event Action<RoomData> OnRoomStarted;
    public static event Action<int, int> OnKeyProgress;     // collected, required
    public static event Action OnRoomCompleted;

    [SerializeField] private RoomData currentRoom;

    private int collectedKeysCount;

    public RoomData CurrentRoom
    {
        get { return currentRoom; }
    }

    public bool IsRoomCompleted
    {
        get
        {
            if (currentRoom == null)
            {
                return false;
            }

            return collectedKeysCount >= currentRoom.requiredKeysCount;
        }
    }

    private void OnEnable()
    {
        KeyItem.OnKeyCollected += HandleKeyCollected;
    }

    private void OnDisable()
    {
        KeyItem.OnKeyCollected -= HandleKeyCollected;
    }

    public void StartRoom(RoomData room)
    {
        currentRoom = room;
        collectedKeysCount = 0;

        if (currentRoom == null)
        {
            Debug.LogWarning("StartRoom called with null RoomData.");
            return;
        }

        if (OnRoomStarted != null)
        {
            OnRoomStarted.Invoke(currentRoom);
        }

        if (OnKeyProgress != null)
        {
            OnKeyProgress.Invoke(collectedKeysCount, currentRoom.requiredKeysCount);
        }
    }

    private void HandleKeyCollected()
    {
        if (currentRoom == null)
        {
            return;
        }

        collectedKeysCount = collectedKeysCount + 1;

        if (OnKeyProgress != null)
        {
            OnKeyProgress.Invoke(collectedKeysCount, currentRoom.requiredKeysCount);
        }

        if (collectedKeysCount >= currentRoom.requiredKeysCount)
        {
            if (OnRoomCompleted != null)
            {
                OnRoomCompleted.Invoke();
            }
        }
    }
}