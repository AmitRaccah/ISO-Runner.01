using UnityEngine;

public class RoomAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip corridorMusic;
    [SerializeField] private bool loop = true;
    [SerializeField] private float volume = 1f;
    [SerializeField] private bool fade = true;
    [SerializeField] private RoomManager roomManager;

    private void Awake()
    {
        if (roomManager == null)
        {
            roomManager = FindObjectOfType<RoomManager>();
        }
    }

    private void OnEnable()
    {
        RoomManager.OnRoomStarted += HandleRoomStarted;
        ExitGateTrigger.OnPlayerPassedExitGate += HandleExitGate;
        TryPlayCurrentRoom();
    }

    private void OnDisable()
    {
        RoomManager.OnRoomStarted -= HandleRoomStarted;
        ExitGateTrigger.OnPlayerPassedExitGate -= HandleExitGate;
    }

    private void HandleRoomStarted(RoomData room)
    {
        if (room == null)
        {
            return;
        }

        AudioClip roomClip = room.roomMusic;
        if (roomClip == null)
        {
            return;
        }

        AudioService audioService = AudioService.Instance;
        if (audioService == null)
        {
            return;
        }

        audioService.PlayMusic(roomClip, loop, volume, fade);
    }

    private void HandleExitGate(RoomData room)
    {
        if (corridorMusic == null)
        {
            return;
        }

        AudioService audioService = AudioService.Instance;
        if (audioService == null)
        {
            return;
        }

        audioService.PlayMusic(corridorMusic, loop, volume, fade);
    }

    private void TryPlayCurrentRoom()
    {
        if (roomManager == null)
        {
            return;
        }

        RoomData currentRoom = roomManager.CurrentRoom;
        if (currentRoom == null)
        {
            return;
        }

        HandleRoomStarted(currentRoom);
    }
}
