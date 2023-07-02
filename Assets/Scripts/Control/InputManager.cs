using PirateGame.Movement;
using UnityEngine;

namespace PirateGame.Control{
    public class InputManager : MonoBehaviour
    {
        // Will be used later.
        private KeyCode increaseSail;
        private KeyCode leftRudder;
        private KeyCode rightRudder;
        private KeyCode reefSail;

        [SerializeField]
        private Sail _sail;
        [SerializeField]
        private Crew _crew;

        public void Setup()
        {
            // Setup the inputkeycode scriptable object if it's not aleady been done.
        }

        private void Update(){
            MovementInput();
        }

        private void MovementInput()
        {
            if (Input.GetKey(KeyCode.W)){_sail.SailStateIncrease();}
            if (Input.GetKey(KeyCode.S)){_sail.SailStateDecrease();}
            if (Input.GetKeyDown(KeyCode.A)){_crew.SetLeftTurn(true);}
            if (Input.GetKeyUp(KeyCode.A)){_crew.SetLeftTurn(false);}
            if (Input.GetKeyDown(KeyCode.D)){_crew.SetRightTurn(true);}
            if (Input.GetKeyUp(KeyCode.D)){_crew.SetRightTurn(false);}
        }
    }
}