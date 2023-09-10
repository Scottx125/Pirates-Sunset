using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Targetting : MonoBehaviour
{
    Vector3 _lastPosition;
    string _name;
    Vector3 _lastSuccessfulPoint;

    private void OnDrawGizmosSelected()
    {
        // Draw a wireframe sphere at the specified position
        Gizmos.color = Color.black; // You can change the color if desired
        if (_lastPosition != null)
        {
            Gizmos.DrawWireSphere(_lastSuccessfulPoint, 10f); // 0.5f is the radius of the sphere
        }

    }
    ///<summary>
    ///Returns a Vector3 position, predicting the impact location of your projectile based on it's speed and the targets position.
    ///</summary>
    public Vector3 Target(Transform target, float projectileSpeed){
        if (_lastPosition == null){
            _lastPosition = target.position;
            _name = target.name;
        }
        // If the last position isn't that of the current target.
        // Send the current targets current position.
        if (target.name != _name){
            _lastPosition = target.position;
            _name = target.name;
            _lastSuccessfulPoint = target.position;
            return target.position;
        }
        // Target is the last target, so predict how far infront we need to aim
        // in order to hit the target.
        // Get relative distance.
        Vector3 relativeDistance = target.position - transform.position;
        // Calculate time taken for projectile to reach object.
        float timeToReachTarget = relativeDistance.magnitude / projectileSpeed;
        // Calculate the targets speed.
        float targetSpeed = (target.position - _lastPosition).magnitude / Time.deltaTime;
        // Some frames happen so fast the ship doesn't have time to move, if that happens return the last successful frame.
        //if (targetSpeed == 0 && target.tag != "Static_Target")
        //{
        //    return _lastSuccessfulPoint;
        //}
        // Future position
        Vector3 predictedIntersection = target.position + target.forward * targetSpeed * timeToReachTarget;

        _lastPosition = target.position;
        _lastSuccessfulPoint = predictedIntersection;
        return predictedIntersection;
    }
}
