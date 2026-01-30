using UnityEngine;

public class FollowPositionOnly : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void LateUpdate()
    {
        if (!target) return;
        transform.position = target.position;
    }
}