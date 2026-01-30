using UnityEngine;

public class MonsterChaser : MonoBehaviour
{
    [SerializeField] private MonoBehaviour trailSource;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float reachDistance = 0.1f;

    private ITrailProvider trailProvider;

    private Vector3 currentTarget;
    private bool hasTarget;

private void Awake()
{
    if (trailSource == null)
    {
        Debug.LogError("TrailSource is NULL. Drag the PlayerTrailRecorder component here.", this);
        return;
    }

    Debug.Log("TrailSource component type is: " + trailSource.GetType().FullName, this);

    trailProvider = trailSource as ITrailProvider;

    if (trailProvider == null)
    {
        Debug.LogError("TrailSource does NOT implement ITrailProvider.", this);
    }
    else
    {
        Debug.Log("TrailSource OK. ITrailProvider connected.", this);
    }
}
    private void Update()
    {
        if (trailProvider == null)
        {
            return;
        }

        if (hasTarget == false)
        {
            Vector3 nextPoint;
            if (trailProvider.TryDequeueNext(out nextPoint) == false)
            {
                return;
            }

            currentTarget = nextPoint;
            hasTarget = true;
        }

        Vector3 pos = transform.position;
        transform.position = Vector3.MoveTowards(pos, currentTarget, moveSpeed * Time.deltaTime);

        float dist = Vector3.Distance(transform.position, currentTarget);
        if (dist <= reachDistance)
        {
            hasTarget = false;
        }
    }
}