using UnityEngine;

[CreateAssetMenu(fileName = "NewSlow", menuName = "Runner/SlowData")]
public class SlowData : ScriptableObject
{
    public float decreaseBy;     
    public float durationSeconds; 
}