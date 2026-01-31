using UnityEngine;

public class RoomAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip corridorMusic;
    [SerializeField] private bool loop = true;
    [SerializeField] private float volume = 1f;
    [SerializeField] private bool fade = true;

    private void OnEnable()
    {
        RoomManager.OnRoomStarted += HandleRoomStarted;
        ExitGateTrigger.OnPlayerPassedExitGate += HandleExitGate;
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
}
