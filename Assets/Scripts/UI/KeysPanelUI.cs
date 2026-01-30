using TMPro;
using UnityEngine;

public class KeysPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TMP_Text keysText;

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
        RoomManager.OnKeyProgress += HandleKeyProgress;
    }

    private void OnDisable()
    {
        RoomManager.OnRoomStarted -= HandleRoomStarted;
        RoomManager.OnRoomCompleted -= HandleRoomCompleted;
        RoomManager.OnKeyProgress -= HandleKeyProgress;
    }

    private void HandleRoomStarted(RoomData room)
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(true);
        }

        UpdateText(0, room.requiredKeysCount);
    }

    private void HandleRoomCompleted()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
        }
    }

    private void HandleKeyProgress(int collected, int required)
    {
        UpdateText(collected, required);
    }

    private void UpdateText(int collected, int required)
    {
        if (keysText == null)
        {
            return;
        }

        keysText.text = collected.ToString() + "/" + required.ToString();
    }
}