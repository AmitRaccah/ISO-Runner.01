using UnityEngine;

public class HazardSideToSide : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float distance = 2f;
    [SerializeField] private float speed = 2f;

    [Header("Space")]
    [SerializeField] private bool useLocalSpace = true;

    private Vector3 startPos;

    private void Start()
    {
        if (useLocalSpace)
        {
            startPos = transform.localPosition;
        }
        else
        {
            startPos = transform.position;
        }
    }

    private void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);
        float offset = Mathf.Lerp(-distance, distance, t);

        Vector3 pos = startPos + (Vector3.right * offset);

        if (useLocalSpace)
        {
            transform.localPosition = pos;
        }
        else
        {
            transform.position = pos;
        }
    }
}