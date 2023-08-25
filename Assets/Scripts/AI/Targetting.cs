using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetting : MonoBehaviour
{
    Vector3 _lastPosition;
    string _name;

    private void OnDrawGizmosSelected()
    {
        // Draw a wireframe sphere at the specified position
        Gizmos.color = Color.black; // You can change the color if desired
        if (_lastPosition != null)
        {
            Gizmos.DrawWireSphere(_lastPosition, 10f); // 0.5f is the radius of the sphere
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
        // Predict based on the above calculations.
        Vector3 predictedIntersection = target.position + target.forward * (targetSpeed * timeToReachTarget);

        _lastPosition = target.position;
        return predictedIntersection;
    }
}
