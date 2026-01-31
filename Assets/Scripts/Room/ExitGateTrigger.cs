using System;
using UnityEngine;

public class ExitGateTrigger : MonoBehaviour
{
    public static event Action<RoomData> OnPlayerPassedExitGate;

    [SerializeField] private RoomData room;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ExitGateTrigger HIT by: " + other.name, this);

        if (!other.CompareTag("Player"))
        {
            Debug.Log("ExitGateTrigger: NOT Player tag", this);
            return;
        }

        if (room == null)
        {
            Debug.Log("ExitGateTrigger: room is NULL", this);
            return;
        }

        Debug.Log("ExitGateTrigger: INVOKE room = " + room.name, this);

        if (OnPlayerPassedExitGate != null)
        {
            OnPlayerPassedExitGate.Invoke(room);
        }
        else
        {
            Debug.Log("ExitGateTrigger: no listeners", this);
        }
    }
}