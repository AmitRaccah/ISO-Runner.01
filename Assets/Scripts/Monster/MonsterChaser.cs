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

    [Header("Boost (distance gated)")]
    [SerializeField] private float boostStartDistance = 10f;
    [SerializeField] private float boostStopDistance = 9f;
    [SerializeField] private float boostSpeed = 12f;

    [Header("Waiting Move")]
    [SerializeField] private float waitingMoveSpeed = 6f;
    [SerializeField] private float waitReachDistance = 0.1f;

    [Header("Game Start")]
    [Tooltip("If true, monster won't chase until GameStartManager starts the game.")]
    [SerializeField] private bool waitForGameStart = true;

    private ITrailProvider trailProvider;

    private Vector3 currentTarget;
    private bool hasTarget;

    private Coroutine routine;
    private WaitForSeconds waitTick;

    private bool isCloseMode;

    // WAIT state (rooms / or game start)
    private bool isWaiting;
    private bool isGoingToWaitPoint;
    private Transform waitPoint;

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

        if (waitForGameStart)
        {
            // Start in waiting mode until the start trigger fires
            isWaiting = true;
            isGoingToWaitPoint = false;
            waitPoint = null;
        }
    }

    private void OnEnable()
    {
        if (waitForGameStart)
        {
            GameStartManager.OnGameStarted += HandleGameStarted;
        }

        routine = StartCoroutine(ChaseRoutine());
    }

    private void OnDisable()
    {
        if (waitForGameStart)
        {
            GameStartManager.OnGameStarted -= HandleGameStarted;
        }

        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }
    }

    private void HandleGameStarted()
    {
        // Start chasing now (no boost forced here; boost is distance-gated anyway)
        isWaiting = false;
        isGoingToWaitPoint = false;
        waitPoint = null;

        isCloseMode = false;
        hasTarget = false;

        if (trailProvider != null)
        {
            trailProvider.Clear();
        }

        StopBoostState();
    }

    // --- API: called from room logic ---

    public void SetWaiting(Transform outsideWaitPoint)
    {
        isWaiting = true;
        waitPoint = outsideWaitPoint;

        isGoingToWaitPoint = (waitPoint != null);

        isCloseMode = false;
        hasTarget = false;

        if (trailProvider != null)
        {
            trailProvider.Clear();
        }

        StopBoostState();
    }

    public void ResumeChaseWithBoost()
    {
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

            // If waiting and need to walk to wait point
            if (isWaiting && isGoingToWaitPoint && waitPoint != null)
            {
                Vector3 targetPos = waitPoint.position;

                float stepWait = waitingMoveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, stepWait);

                float dist = Vector3.Distance(transform.position, targetPos);
                if (dist <= waitReachDistance)
                {
                    isGoingToWaitPoint = false; // stay idle at wait point
                }

                yield return null;
                continue;
            }

            // If waiting (idle)
            if (isWaiting)
            {
                yield return null;
                continue;
            }

            float distToPlayer = GetDistanceToPlayer();

            // Boost hysteresis
            if (isBoosting == false && distToPlayer >= boostStartDistance)
            {
                StartBoostState();
            }
            else if (isBoosting == true && distToPlayer <= boostStopDistance)
            {
                StopBoostState();
            }

            // Close mode enter/exit
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
        if (monsterCollider != null && playerCollider != null)
        {
            Vector3 aPoint = monsterCollider.ClosestPoint(player.position);
            Vector3 bPoint = playerCollider.ClosestPoint(transform.position);
            return Vector3.Distance(aPoint, bPoint);
        }

        return Vector3.Distance(transform.position, player.position);
    }

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