using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PirateGame.Moving;

public class AIInputManager : MonoBehaviour
{
    private MovementManager _movementManager;
    private IFireCannons _fireCannons;

    public void Setup(MovementManager movementManager, IFireCannons fireCannons)
    {
        if (_movementManager == null) _movementManager = movementManager;
        if (_fireCannons == null) _fireCannons = fireCannons;
    }

    public void Fire(CannonPositionEnum direction, AmmunitionTypeEnum ammoType)
    {
        // Get direction to fire and fire (this will be direction closest to current target which will be passed in)
        // Also get the type of ammo to fire.
    }

    // Input a nullable custom state to specify a new speed OR indicate if you want the speed to increase or decrease.
    public void MovementInput(SpeedModifierEnum speed)
    {
        _movementManager.ChangeSpeed(speed, null);
    }

    public void Rotation(Vector3 currentWaypoint, float targetRelativeAngle)
    {
        // Calc direciton to waypoint
        Vector3 directionToWayPoint = currentWaypoint - transform.position;
        // Calc angle between forward vector and the direction.
        float angleToWayPoint = Vector3.Angle(transform.forward, directionToWayPoint);
        // If the angle is large enough to warrent turning.
        if (angleToWayPoint > targetRelativeAngle)
        {
            // crossproduct to determine if the angle is left or right of the forward vector.
            Vector3 crossProduct = Vector3.Cross(transform.forward, directionToWayPoint);
            // Turn left or right.
            if (crossProduct.y < 0)
            {
                _movementManager.TurnLeft(true);
                _movementManager.TurnRight(false);
            } else
            {
                _movementManager.TurnLeft(false);
                _movementManager.TurnRight(true);
            }
        } else
        {
            // If the angle is too small don't bother turning at all.
            _movementManager.TurnLeft(false);
            _movementManager.TurnRight(false);
        }
        
    }
    
}
