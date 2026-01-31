using UnityEngine;

public class SceneMusicStarter : MonoBehaviour
{
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private bool loop = true;
    [SerializeField] private float volume = 1f;
    [SerializeField] private bool fade = true;

    private void Start()
    {
        if (musicClip == null)
        {
            return;
        }

        AudioService audioService = AudioService.Instance;
        if (audioService == null)
        {
            Debug.LogWarning("SceneMusicStarter: AudioService not found.", this);
            return;
        }

        audioService.PlayMusic(musicClip, loop, volume, fade);
    }
}
