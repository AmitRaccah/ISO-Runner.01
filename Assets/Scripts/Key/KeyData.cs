using UnityEngine;

[CreateAssetMenu(fileName = "NewKey", menuName = "Runner/KeyData")]
public class KeyData : ScriptableObject
{
    public string keyName;
//ATTACH TO SPACIFIC ROOM
    public string roomId;

    public Color debugColor = Color.cyan;
}