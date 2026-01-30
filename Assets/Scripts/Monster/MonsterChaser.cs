using UnityEngine;
using System.Collections;

public class MonsterChaser : MonoBehaviour
{
    [SerializeField] private MonoBehaviour trailSource;
    [SerializeField] private Transform player;

    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float reachDistance = 0.1f;

    [SerializeField] private float catchUpDistance = 8f;
    [SerializeField] private int maxSkipsPerTick = 10;

    [SerializeField] private float tickInterval = 0.02f;

    private ITrailProvider trailProvider;

    private Vector3 currentTarget;
    private bool hasTarget;

    private Coroutine routine;

    private void Awake()
    {
        trailProvider = trailSource as ITrailProvider;

        if (trailProvider == null)
        {
            Debug.LogError("TrailSource does NOT implement ITrailProvider.", this);
        }

        if (player == null)
        {
            Debug.LogError("Player is NULL. Assign the player Transform.", this);
        }
    }

    private void OnEnable()
    {
        routine = StartCoroutine(ChaseRoutine());
    }

    private void OnDisable()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }
    }

    private IEnumerator ChaseRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(tickInterval);

        while (true)
        {
            yield return wait;

            if (trailProvider == null || player == null)
            {
                continue;
            }

            if (hasTarget == false)
            {
                if (TryPickTarget() == false)
                {
                    continue;
                }

                hasTarget = true;
            }

            float step = moveSpeed * tickInterval;
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, step);

            float distToTarget = Vector3.Distance(transform.position, currentTarget);
            if (distToTarget <= reachDistance)
            {
                hasTarget = false;
            }
        }
    }

    private bool TryPickTarget()
    {
        Vector3 nextPoint;
        if (trailProvider.TryDequeueNext(out nextPoint) == false)
        {
            return false;
        }

        currentTarget = nextPoint;

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer >= catchUpDistance)
        {
            int skips = 0;

            while (skips < maxSkipsPerTick)
            {
                Vector3 newerPoint;
                if (trailProvider.TryDequeueNext(out newerPoint) == false)
                {
                    break;
                }

                currentTarget = newerPoint;
                skips = skips + 1;
            }
        }

        return true;
    }
}