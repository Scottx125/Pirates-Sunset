using PirateGame.Movement;
using UnityEngine;

namespace PirateGame.Control{
    public class PlayerInputManager : MonoBehaviour
    {
        // Will be used later.
        private KeyCode increaseSail;
        private KeyCode leftRudder;
        private KeyCode rightRudder;
        private KeyCode reefSail;

        [SerializeField]
        private CannonManager _cannonManager;
        [SerializeField]
        private MovementManager _movementManager;


        public void Setup(MovementManager movementManager)
        {
            if (_movementManager == null) _movementManager = movementManager;
            // Use a SO and change it, it will save the data automatically.
        }

        private void Update(){
            MovementInput();
        }

        private void MovementInput()
        {
            if (Input.GetKey(KeyCode.W)){_movementManager.IncreaseSpeed();}
            if (Input.GetKey(KeyCode.S)){_movementManager.DecreaseSpeed();}
            if (Input.GetKeyDown(KeyCode.A)){_movementManager.TurnLeft(true);}
            if (Input.GetKeyUp(KeyCode.A)){_movementManager.TurnLeft(false);}
            if (Input.GetKeyDown(KeyCode.D)){_movementManager.TurnRight(true);}
            if (Input.GetKeyUp(KeyCode.D)){_movementManager.TurnRight(false);}
        }
    }
}