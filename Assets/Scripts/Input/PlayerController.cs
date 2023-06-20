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

        private void Awake()
        {
            Setup();
        }

        private void Update(){
            DetectInput();
        }

        private void DetectInput()
        {
            if (Input.GetKey(KeyCode.W)){mover.SailStateIncrease();}// Do something
            if (Input.GetKey(KeyCode.A)){}// Do something
            if (Input.GetKey(KeyCode.D)){}// Do something
            if (Input.GetKey(KeyCode.S)){mover.SailStateDecrease();}// Do something
        }

        private void Setup()
        {
            // Setup the inputkeycode scriptable object if it's not aleady been done.

            // Setup GetComponenets if not already attatched in the inspector.
            if (mover == null){mover = GetComponent<Mover>();}
        }
    }
}