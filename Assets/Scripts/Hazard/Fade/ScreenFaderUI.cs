using System.Collections;
using UnityEngine;

public class ScreenFaderUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private Coroutine routine;

    private void Awake()
    {
        if (canvasGroup == null)
        {
            Debug.LogError("ScreenFaderUI: CanvasGroup is not assigned.", this);
            return;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    private void OnEnable()
    {
        FadeZone.OnFadeRequested += HandleFadeRequested;
    }

    private void OnDisable()
    {
        FadeZone.OnFadeRequested -= HandleFadeRequested;
    }

    private void HandleFadeRequested(FadeData data)
    {
        if (canvasGroup == null || data == null)
        {
            return;
        }

        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }

        routine = StartCoroutine(FadeSequence(data));
    }

    private IEnumerator FadeSequence(FadeData data)
    {
        yield return FadeTo(1f, data.fadeOutSeconds);

        float hold = data.holdBlackSeconds;
        if (hold > 0f)
        {
            yield return new WaitForSeconds(hold);
        }

        yield return FadeTo(0f, data.fadeInSeconds);

        routine = null;
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;

        if (duration <= 0f)
        {
            canvasGroup.alpha = targetAlpha;
            yield break;
        }

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = t / duration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, p);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}