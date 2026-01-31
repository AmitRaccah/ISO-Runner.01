using System;
using UnityEngine;

public class MonsterCatchTrigger : MonoBehaviour
{
    public static event Action OnPlayerCaught;

    [SerializeField] private string playerTag = "Player";

    private bool hasCaught;

    private void OnTriggerEnter(Collider other)
    {
        if (hasCaught)
        {
            return;
        }

        if (other == null)
        {
            return;
        }

        // 1) direct tag
        if (other.CompareTag(playerTag))
        {
            Catch();
            return;
        }

        // 2) root tag (for cases like PlayerArmature / child colliders)
        Transform root = other.transform.root;
        if (root != null && root.CompareTag(playerTag))
        {
            Catch();
            return;
        }

        // 3) parent chain (optional safety)
        Transform parent = other.transform;
        while (parent != null)
        {
            if (parent.CompareTag(playerTag))
            {
                Catch();
                return;
            }

            parent = parent.parent;
        }
    }

    private void Catch()
    {
        hasCaught = true;

        if (OnPlayerCaught != null)
        {
            OnPlayerCaught.Invoke();
        }
    }
}