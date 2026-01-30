using System;
using System.Collections;
using UnityEngine;

public class TimerService : MonoBehaviour
{
    public static event Action<float> OnTimeChanged;

    private Coroutine timerRoutine;
    private float remainingSeconds;

    private void OnEnable()
    {
        RoomManager.OnRoomStarted += HandleRoomStarted;
        RoomManager.OnRoomCompleted += HandleRoomCompleted;
    }

    private void OnDisable()
    {
        RoomManager.OnRoomStarted -= HandleRoomStarted;
        RoomManager.OnRoomCompleted -= HandleRoomCompleted;
    }

    private void HandleRoomStarted(RoomData room)
    {
        StartTimer(room.timeLimitSeconds);
    }

    private void HandleRoomCompleted()
    {
        StopTimer();
    }

    private void StartTimer(float seconds)
    {
        StopTimer();

        remainingSeconds = seconds;
        if (remainingSeconds < 0f)
        {
            remainingSeconds = 0f;
        }

        timerRoutine = StartCoroutine(TimerRoutine());
    }

    private void StopTimer()
    {
        if (timerRoutine != null)
        {
            StopCoroutine(timerRoutine);
            timerRoutine = null;
        }
    }

    private IEnumerator TimerRoutine()
    {
        float tick = 0.1f;
        float nextLogTime = Mathf.Floor(remainingSeconds);

        Debug.Log("TIMER START: " + remainingSeconds.ToString("0.0"));

        if (OnTimeChanged != null)
        {
            OnTimeChanged.Invoke(remainingSeconds);
        }

        while (remainingSeconds > 0f)
        {
            yield return new WaitForSeconds(tick);

            remainingSeconds = remainingSeconds - tick;
            if (remainingSeconds < 0f)
            {
                remainingSeconds = 0f;
            }

            if (OnTimeChanged != null)
            {
                OnTimeChanged.Invoke(remainingSeconds);
            }

            if (remainingSeconds <= nextLogTime)
            {
                Debug.Log("TIME LEFT: " + remainingSeconds.ToString("0.0"));
                nextLogTime = nextLogTime - 1f;
            }
        }

        Debug.Log("TIMER END");
        timerRoutine = null;
    }
}