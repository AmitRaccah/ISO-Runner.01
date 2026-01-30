using UnityEngine;
using System.Collections;

public class MonsterChaser : MonoBehaviour
{
    [SerializeField] private MonoBehaviour trailSource;
    [SerializeField] private Transform player;

    [Header("Base Move")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float reachDistance = 0.1f;

    [Header("Trail")]
    [SerializeField] private float catchUpDistance = 8f;
    [SerializeField] private int maxSkipsPerStep = 10;

    [Header("Close Mode")]
    [SerializeField] private float closeEnterDistance = 3f;
    [SerializeField] private float closeExitDistance = 5f;

    [Header("Tick")]
    [SerializeField] private float tickInterval = 0.05f;

    [Header("Optional Colliders (can be null)")]
    [SerializeField] private Collider monsterCollider;
    [SerializeField] private Collider playerCollider;

    [Header("Boost")]
    [SerializeField] private float boostStartDistance = 10f;
    [SerializeField] private float boostStopDistance = 9f;
    [SerializeField] private float boostSpeed = 12f;

    [Header("Waiting Move")]
    [SerializeField] private float waitingMoveSpeed = 6f;
    [SerializeField] private float waitReachDistance = 0.1f;

    private ITrailProvider trailProvider;

    private Vector3 currentTarget;
    private bool hasTarget;

    private Coroutine routine;
    private WaitForSeconds waitTick;

    private bool isCloseMode;

    // WAIT state
    private bool isWaiting;
    private Transform waitPoint;
    private bool isGoingToWaitPoint;

    // Speed state
    private float currentSpeed;
    private bool isBoosting;

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

        // colliders are OPTIONAL
        if (monsterCollider == null)
        {
            monsterCollider = GetComponent<Collider>();
        }

        if (playerCollider == null && player != null)
        {
            playerCollider = player.GetComponent<Collider>();
        }

        waitTick = new WaitForSeconds(tickInterval);

        currentSpeed = moveSpeed;
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

    // --- API: called from room logic ---

    public void SetWaiting(Transform outsideWaitPoint)
    {
        Debug.Log("MonsterChaser: SetWaiting called. waitPoint=" + (outsideWaitPoint != null ? outsideWaitPoint.name : "NULL"), this);

        isWaiting = true;
        isGoingToWaitPoint = true;
        waitPoint = outsideWaitPoint;

        isCloseMode = false;
        hasTarget = false;

        if (trailProvider != null)
        {
            trailProvider.Clear();
        }

    }

    public void ResumeChaseWithBoost()
    {
        Debug.Log("MonsterChaser: ResumeChaseWithBoost called.", this);

        isWaiting = false;
        isGoingToWaitPoint = false;
        waitPoint = null;

        isCloseMode = false;
        hasTarget = false;

        if (trailProvider != null)
        {
            trailProvider.Clear();
        }

        StartBoostState();
    }

    // --- core loop ---

    private IEnumerator ChaseRoutine()
    {
        while (true)
        {
            if (trailProvider == null || player == null)
            {
                yield return null;
                continue;
            }

            if (isWaiting && isGoingToWaitPoint && waitPoint != null)
            {
                float stepToWait = waitingMoveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, waitPoint.position, stepToWait);

                float distToWait = Vector3.Distance(transform.position, waitPoint.position);
                if (distToWait <= waitReachDistance)
                {
                    isGoingToWaitPoint = false;
                }

                yield return null;
                continue;
            }

            // if waiting outside room -> do nothing
            if (isWaiting)
            {
                yield return null;
                continue;
            }

            float distToPlayer = GetDistanceToPlayer();

            if (isBoosting == false && distToPlayer >= boostStartDistance)
            {
                StartBoostState();
            }
            else if (isBoosting == true && distToPlayer <= boostStopDistance)
            {
                StopBoostState();
            }

            // your original close logic (unchanged)
            if (isCloseMode == false && distToPlayer <= closeEnterDistance)
            {
                isCloseMode = true;
                hasTarget = false;
            }
            else if (isCloseMode == true && distToPlayer >= closeExitDistance)
            {
                isCloseMode = false;
                hasTarget = false;

                trailProvider.Clear();
            }

            if (isCloseMode)
            {
                yield return null;

                currentTarget = player.position;

                float stepClose = currentSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, currentTarget, stepClose);

                continue;
            }

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

        float step = currentSpeed * tickInterval;
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

    private float GetDistanceToPlayer()
    {
        // If both colliders exist -> accurate body-to-body distance
        if (monsterCollider != null && playerCollider != null)
        {
            Vector3 aPoint = monsterCollider.ClosestPoint(player.position);
            Vector3 bPoint = playerCollider.ClosestPoint(transform.position);
            return Vector3.Distance(aPoint, bPoint);
        }

        // fallback -> center distance
        return Vector3.Distance(transform.position, player.position);
    }

    // --- boost ---

    private void StartBoostState()
    {
        isBoosting = true;
        currentSpeed = boostSpeed;
    }

    private void StopBoostState()
    {
        isBoosting = false;
        currentSpeed = moveSpeed;
    }
}
