using TMPro;
using UnityEngine;

public class TimePanelUI : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TMP_Text timeText;

    private void Awake()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
        }
    }

    private void OnEnable()
    {
        RoomManager.OnRoomStarted += HandleRoomStarted;
        RoomManager.OnRoomCompleted += HandleRoomCompleted;
        TimerService.OnTimeChanged += HandleTimeChanged;
    }

    private void OnDisable()
    {
        RoomManager.OnRoomStarted -= HandleRoomStarted;
        RoomManager.OnRoomCompleted -= HandleRoomCompleted;
        TimerService.OnTimeChanged -= HandleTimeChanged;
    }

    private void HandleRoomStarted(RoomData room)
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(true);
        }
    }

    private void HandleRoomCompleted()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
        }
    }

    private void HandleTimeChanged(float secondsRemaining)
    {
        if (timeText == null)
        {
            return;
        }

        if (secondsRemaining < 0f)
        {
            secondsRemaining = 0f;
        }

        int secondsInt = Mathf.CeilToInt(secondsRemaining);
        int minutes = secondsInt / 60;
        int seconds = secondsInt % 60;

        timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}