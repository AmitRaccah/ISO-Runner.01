using System.Collections.Generic;
using UnityEngine;

public class PlayerTrailRecorder : MonoBehaviour, ITrailProvider
{
    [SerializeField] private float recordDistance = 0.25f;

    private Queue<Vector3> points = new Queue<Vector3>();
    private Vector3 lastRecordedPos;

    private void Start()
    {
        lastRecordedPos = transform.position;
        points.Enqueue(lastRecordedPos);
    }

    private void Update()
    {
        Vector3 currentPos = transform.position;

        Vector3 delta = currentPos - lastRecordedPos;
        float dist = delta.magnitude;

        if (dist < recordDistance)
        {
            return;
        }

        Vector3 deltaDir = delta / dist;
        float dot = Vector3.Dot(deltaDir, transform.forward);

        if (dot <= 0f)
        {
            return;
        }

        lastRecordedPos = currentPos;
        points.Enqueue(currentPos);
    }

    public bool TryDequeueNext(out Vector3 point)
    {
        if (points.Count <= 0)
        {
            point = Vector3.zero;
            return false;
        }

        point = points.Dequeue();
        return true;
    }

    public void Clear()
    {
        points.Clear();
        lastRecordedPos = transform.position;
        points.Enqueue(lastRecordedPos);
    }
}