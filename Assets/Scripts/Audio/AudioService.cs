using System.Collections;
using UnityEngine;

public class AudioService : MonoBehaviour
{
    private static AudioService _instance;

    public static AudioService Instance
    {
        get { return _instance; }
    }

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Defaults")]
    [SerializeField] private float defaultFadeSeconds = 0.35f;

    private Coroutine musicFadeRoutine;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        musicSource.playOnAwake = false;
        musicSource.loop = true;

        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
    }

    public void PlayMusic(AudioClip clip, bool loop, float volume, bool fade)
    {
        if (clip == null)
        {
            return;
        }

        if (musicSource == null)
        {
            return;
        }

        if (musicFadeRoutine != null)
        {
            StopCoroutine(musicFadeRoutine);
            musicFadeRoutine = null;
        }

        if (fade == false)
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = volume;
            musicSource.Play();
            return;
        }

        musicFadeRoutine = StartCoroutine(FadeToMusic(clip, loop, volume));
    }

    public void StopMusic(bool fade)
    {
        if (musicSource == null)
        {
            return;
        }

        if (musicFadeRoutine != null)
        {
            StopCoroutine(musicFadeRoutine);
            musicFadeRoutine = null;
        }

        if (fade == false)
        {
            musicSource.Stop();
            musicSource.clip = null;
            return;
        }

        musicFadeRoutine = StartCoroutine(FadeOutAndStop());
    }

    public void PlaySfx(AudioClip clip, float volume)
    {
        if (clip == null)
        {
            return;
        }

        if (sfxSource == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip, volume);
    }

    private IEnumerator FadeToMusic(AudioClip clip, bool loop, float targetVolume)
    {
        float duration = defaultFadeSeconds;

        if (duration <= 0f)
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = targetVolume;
            musicSource.Play();
            yield break;
        }

        float startVolume = musicSource.volume;

        if (musicSource.isPlaying)
        {
            float t = 0f;
            while (t < duration)
            {
                t = t + Time.deltaTime;
                float lerp = t / duration;
                musicSource.volume = Mathf.Lerp(startVolume, 0f, lerp);
                yield return null;
            }
        }

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = 0f;
        musicSource.Play();

        float tIn = 0f;
        while (tIn < duration)
        {
            tIn = tIn + Time.deltaTime;
            float lerpIn = tIn / duration;
            musicSource.volume = Mathf.Lerp(0f, targetVolume, lerpIn);
            yield return null;
        }

        musicSource.volume = targetVolume;
        musicFadeRoutine = null;
    }

    private IEnumerator FadeOutAndStop()
    {
        float duration = defaultFadeSeconds;

        if (duration <= 0f)
        {
            musicSource.Stop();
            musicSource.clip = null;
            yield break;
        }

        float startVolume = musicSource.volume;
        float t = 0f;

        while (t < duration)
        {
            t = t + Time.deltaTime;
            float lerp = t / duration;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, lerp);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = null;
        musicFadeRoutine = null;
    }
}
