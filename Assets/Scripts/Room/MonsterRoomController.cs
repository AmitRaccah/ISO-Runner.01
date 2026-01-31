using UnityEngine;

public class MonsterRoomController : MonoBehaviour
{
    [SerializeField] private MonsterChaser monster;
    [SerializeField] private RoomManager roomManager;

    private bool isPlayerInRoom;

    private void OnEnable()
    {
        RoomStarter.OnPlayerEnteredRoom += HandlePlayerEnteredRoom;
        ExitGateTrigger.OnPlayerPassedExitGate += HandlePlayerPassedExitGate;
    }

    private void OnDisable()
    {
        RoomStarter.OnPlayerEnteredRoom -= HandlePlayerEnteredRoom;
        ExitGateTrigger.OnPlayerPassedExitGate -= HandlePlayerPassedExitGate;
    }

    private void HandlePlayerEnteredRoom(Transform waitPoint)
    {
        isPlayerInRoom = true;

        if (monster != null && waitPoint != null)
        {
            monster.SetWaiting(waitPoint);
        }
    }

    private void HandlePlayerPassedExitGate(RoomData room)
    {
        if (isPlayerInRoom == false)
        {
            return;
        }

        if (roomManager == null)
        {
            return;
        }

        if (room == null)
        {
            return;
        }

        // must be the active room
        if (roomManager.CurrentRoom == null || roomManager.CurrentRoom != room)
        {
            return;
        }

        // must be completed (keys collected)
        if (roomManager.IsRoomCompleted == false)
        {
            return;
        }

        isPlayerInRoom = false;

        if (monster != null)
        {
            monster.ResumeChaseWithBoost();
        }
    }
}