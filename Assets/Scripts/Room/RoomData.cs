using UnityEngine;

[CreateAssetMenu(fileName = "NewRoom", menuName = "Runner/RoomData")]
public class RoomData : ScriptableObject
{
    public int requiredKeysCount;
    public float timeLimitSeconds;
}