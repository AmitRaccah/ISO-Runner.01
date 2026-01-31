using System;
using UnityEngine;

public class RoomStarter : MonoBehaviour
{
    public static event Action<Transform> OnPlayerEnteredRoom;

    [SerializeField] private RoomData room;
    [SerializeField] private RoomManager roomManager;

    [SerializeField] private Transform monsterWaitPoint;

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

        if (OnPlayerEnteredRoom != null && monsterWaitPoint != null)
        {
            OnPlayerEnteredRoom.Invoke(monsterWaitPoint);
        }

        gameObject.SetActive(false);
    }
}