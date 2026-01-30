using UnityEngine;
using System.Collections;

public class MonsterChaser : MonoBehaviour
{
    [SerializeField] private MonoBehaviour trailSource;
    [SerializeField] private Transform player;

    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float reachDistance = 0.1f;

    [SerializeField] private float catchUpDistance = 8f;
    [SerializeField] private int maxSkipsPerStep = 10;

    [SerializeField] private float closeEnterDistance = 3f; // נכנס למצב קרוב
    [SerializeField] private float closeExitDistance = 5f;  // יוצא ממצב קרוב

    [SerializeField] private float tickInterval = 0.05f;

    [SerializeField] private Collider monsterCollider;
    [SerializeField] private Collider playerCollider;

    private ITrailProvider trailProvider;

    private Vector3 currentTarget;
    private bool hasTarget;

    private Coroutine routine;
    private WaitForSeconds waitTick;

    private bool isCloseMode;

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

        if (monsterCollider == null)
        {
            monsterCollider = GetComponent<Collider>();
        }

        if (playerCollider == null && player != null)
        {
            playerCollider = player.GetComponent<Collider>();
        }

        if (monsterCollider == null)
        {
            Debug.LogError("MonsterCollider is NULL. Assign a Collider on the monster.", this);
        }

        if (playerCollider == null)
        {
            Debug.LogError("PlayerCollider is NULL. Assign a Collider on the player.", this);
        }

        waitTick = new WaitForSeconds(tickInterval);
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
        while (true)
        {
            if (trailProvider == null || player == null || monsterCollider == null || playerCollider == null)
            {
                yield return null;
                continue;
            }

            float distToPlayer = GetColliderDistance(monsterCollider, playerCollider);

            // מצב קרוב: נכנסים מתחת ל-enter
            if (isCloseMode == false && distToPlayer <= closeEnterDistance)
            {
                isCloseMode = true;
                hasTarget = false;
            }
            // יוצאים רק מעל exit
            else if (isCloseMode == true && distToPlayer >= closeExitDistance)
            {
                isCloseMode = false;
                hasTarget = false;

                // קריטי: זורקים מסלול ישן כדי לא לחזור אחורה
                trailProvider.Clear();
            }

            if (isCloseMode)
            {
                yield return null;

                // רדיפה ישירה כשהוא קרוב
                currentTarget = player.position;

                float step = moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, currentTarget, step);

                continue;
            }

            // מצב רחוק: טיק
            yield return waitTick;

            StepTrailChase(distToPlayer);
        }
    }

    private void StepTrailChase(float distToPlayer)
    {
        if (hasTarget == false)
        {
            if (TryPickTrailTarget(distToPlayer) == false)
            {
                return;
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

    private bool TryPickTrailTarget(float distToPlayer)
    {
        Vector3 nextPoint;
        if (trailProvider.TryDequeueNext(out nextPoint) == false)
        {
            return false;
        }

        currentTarget = nextPoint;

        if (distToPlayer >= catchUpDistance)
        {
            int skips = 0;

            while (skips < maxSkipsPerStep)
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

    private float GetColliderDistance(Collider a, Collider b)
    {
        Vector3 aPoint = a.ClosestPoint(b.transform.position);
        Vector3 bPoint = b.ClosestPoint(a.transform.position);
        return Vector3.Distance(aPoint, bPoint);
    }
}