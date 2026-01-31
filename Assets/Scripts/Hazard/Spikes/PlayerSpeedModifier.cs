using System.Collections;
using UnityEngine;
using StarterAssets;

public class PlayerSpeedModifier : MonoBehaviour
{
    [SerializeField] private ThirdPersonController controller;

    private float baseMoveSpeed;
    private float baseSprintSpeed;

    private Coroutine slowRoutine;

    private void Awake()
    {
        if (controller == null)
        {
            controller = GetComponent<ThirdPersonController>();
        }

        if (controller == null)
        {
            Debug.LogError("PlayerSpeedModifier needs a ThirdPersonController reference.", this);
            return;
        }

        baseMoveSpeed = controller.MoveSpeed;
        baseSprintSpeed = controller.SprintSpeed;
    }

    private void OnEnable()
    {
        SpikeHazard.OnPlayerHitHazard += ApplySlow;
    }

    private void OnDisable()
    {
        SpikeHazard.OnPlayerHitHazard -= ApplySlow;
    }

private void ApplySlow(SlowData slowData)
{
    Debug.Log("APPLY SLOW CALLED", this);

    if (controller == null)
    {
        Debug.LogError("ThirdPersonController is NULL", this);
        return;
    }

    if (slowData == null)
    {
        Debug.LogError("SlowData is NULL", this);
        return;
    }

    Debug.Log("SLOW DATA: decreaseBy=" + slowData.decreaseBy + " duration=" + slowData.durationSeconds, this);

    if (slowRoutine != null)
    {
        StopCoroutine(slowRoutine);
        slowRoutine = null;
    }

    slowRoutine = StartCoroutine(SlowRoutine(slowData));
}

    private IEnumerator SlowRoutine(SlowData slowData)
    {
        float decrease = slowData.decreaseBy;

        if (decrease < 0f)
        {
            decrease = 0f;
        }

        float newMove = baseMoveSpeed - decrease;
        float newSprint = baseSprintSpeed - decrease;

        if (newMove < 0f) newMove = 0f;
        if (newSprint < 0f) newSprint = 0f;

        controller.MoveSpeed = newMove;
        controller.SprintSpeed = newSprint;

        float duration = slowData.durationSeconds;
        if (duration < 0f)
        {
            duration = 0f;
        }

        if (duration > 0f)
        {
            yield return new WaitForSeconds(duration);
        }

        controller.MoveSpeed = baseMoveSpeed;
        controller.SprintSpeed = baseSprintSpeed;

        slowRoutine = null;
    }
}