using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Current target to follow
    public Vector3 offset; // Offset of the camera from the target

    private void LateUpdate()
    {
        // Check if the target is valid
        if (target == null)
            return;

        // Set the camera's position to the target's position plus the offset
        transform.position = target.position + offset;
    }

    public void SetTarget(Transform newTarget)
    {
        // Set a new target for the camera to follow
        target = newTarget;
    }
}
