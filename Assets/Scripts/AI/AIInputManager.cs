using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PirateGame.Moving;

public class AIInputManager : MonoBehaviour
{
        [SerializeField]
        private MovementManager _movementManager;
        [SerializeField]
        private IFireCannons _fireCannons;

        public void Setup(MovementManager movementManager, IFireCannons fireCannons)
        {
            if (_movementManager == null) _movementManager = movementManager;
            if (_fireCannons == null) _fireCannons = fireCannons;
        }

        public void Fire(){
            // Get direction to fire and fire (this will be direction closest to current target which will be passed in)
        }

        public void MovementInput(){
            // Simply called to tell movement to increase or decrease speed.
        }
}
