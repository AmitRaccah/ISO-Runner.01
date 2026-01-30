using UnityEngine;

[CreateAssetMenu(fileName = "NewFade", menuName = "Runner/FadeData")]
public class FadeData : ScriptableObject
{
    public float fadeOutSeconds = 0.5f;
    public float holdBlackSeconds = 1.0f;
    public float fadeInSeconds = 0.5f;
}