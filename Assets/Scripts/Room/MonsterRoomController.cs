using UnityEngine;

public class MonsterRoomController : MonoBehaviour
{
    [SerializeField] private MonsterChaser monster;

    private Transform lastWaitPoint;

    private bool isPlayerInRoom;
    private bool isRoomCompleted;

    private void OnEnable()
    {
        RoomStarter.OnPlayerEnteredRoom += HandlePlayerEnteredRoom;
        RoomManager.OnRoomCompleted += HandleRoomCompleted;
        ExitGateTrigger.OnPlayerPassedExitGate += HandlePlayerPassedExitGate;
    }

    private void OnDisable()
    {
        RoomStarter.OnPlayerEnteredRoom -= HandlePlayerEnteredRoom;
        RoomManager.OnRoomCompleted -= HandleRoomCompleted;
        ExitGateTrigger.OnPlayerPassedExitGate -= HandlePlayerPassedExitGate;
    }

    private void HandlePlayerEnteredRoom(Transform waitPoint)
    {
        isPlayerInRoom = true;
        isRoomCompleted = false;

        lastWaitPoint = waitPoint;

        if (monster != null && lastWaitPoint != null)
        {
            monster.SetWaiting(lastWaitPoint);
        }
    }

    private void HandleRoomCompleted()
    {
        isRoomCompleted = true;
    }

    private void HandlePlayerPassedExitGate()
    {
        if (isPlayerInRoom == false)
        {
            return;
        }

        if (isRoomCompleted == false)
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