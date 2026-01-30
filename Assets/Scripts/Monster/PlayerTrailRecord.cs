using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrailRecorder : MonoBehaviour, ITrailProvider
{
    [SerializeField] private float recordDistance = 0.25f;
    [SerializeField] private float sampleInterval = 0.05f;

    private Queue<Vector3> points = new Queue<Vector3>();
    private Vector3 lastRecordedPos;
    private Coroutine routine;

    private void OnEnable()
    {
        lastRecordedPos = transform.position;
        points.Enqueue(lastRecordedPos);

        routine = StartCoroutine(SampleRoutine());
    }

    private void OnDisable()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }
    }

    private IEnumerator SampleRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(sampleInterval);

        while (true)
        {
            yield return wait;

            Vector3 currentPos = transform.position;

            Vector3 delta = currentPos - lastRecordedPos;
            float dist = delta.magnitude;

            if (dist < recordDistance)
            {
                continue;
            }

            Vector3 deltaDir = delta / dist;
            float dot = Vector3.Dot(deltaDir, transform.forward);

            if (dot <= 0f)
            {
                continue;
            }

            lastRecordedPos = currentPos;
            points.Enqueue(currentPos);
        }
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
}