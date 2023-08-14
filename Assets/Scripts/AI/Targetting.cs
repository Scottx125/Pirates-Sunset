using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetting : MonoBehaviour
{
    Transform _lastPosition;
    public Vector3 Target(Transform target, float projectileSpeed){
        if (_lastPosition == null){
            _lastPosition = target;
        }
        // If the last position isn't that of the current target.
        // Send the current targets current position.
        if (target.name != _lastPosition.name){
            _lastPosition = target;
            return target.position;
        }

        // Target is the last target, so predict how far infront we need to aim
        // in order to hit the target.
        // Get relative distance.
        Vector3 relativeDistance = target.position - transform.position;
        // Calculate time taken for projectile to reach object.
        float timeToReachTarget = relativeDistance.magnitude / projectileSpeed;
        // Calculate the targets speed.
        float targetSpeed = (target.position - _lastPosition.position).magnitude / Time.deltaTime;
        // Predict based on the above calculations.
        Vector3 predictedIntersection = target.position + target.forward * (targetSpeed * timeToReachTarget);

        _lastPosition = target;

        return predictedIntersection;
    }
}
