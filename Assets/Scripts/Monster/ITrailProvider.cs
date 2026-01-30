using UnityEngine;

public interface ITrailProvider
{
    bool TryDequeueNext(out Vector3 point);
}