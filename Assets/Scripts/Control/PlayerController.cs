using PirateGame.Movement;
using UnityEngine;

namespace PirateGame.Control{
    public class PlayerController : MonoBehaviour
    {
        // Will be used later.
        private KeyCode increaseSail;
        private KeyCode leftRudder;
        private KeyCode rightRudder;
        private KeyCode reefSail;

        [SerializeField]
        private Mover mover;

        public void Setup()
        {
            // Setup the inputkeycode scriptable object if it's not aleady been done.

            // Setup GetComponenets if not already attatched in the inspector.
            if (mover == null){mover = GetComponent<Mover>();}
        }

        private void Update(){
            DetectInput();
        }

        private void DetectInput()
        {
            if (Input.GetKey(KeyCode.W)){mover.SailStateIncrease();}// Do something
            if (Input.GetKeyDown(KeyCode.A)){mover.LeftTurnEnable();}// Do something
            if (Input.GetKeyDown(KeyCode.D)){mover.RightTurnEnable();}// Do something
            if (Input.GetKeyUp(KeyCode.A)){mover.LeftTurnDisable();}// Do something
            if (Input.GetKeyUp(KeyCode.D)){mover.RightTurnDisable();}// Do something
            if (Input.GetKey(KeyCode.S)){mover.SailStateDecrease();}// Do something
        }
    }
}