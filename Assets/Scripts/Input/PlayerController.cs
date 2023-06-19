using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private InputScriptableObject inputKeycodes;

    private KeyCode increaseSail;
    private KeyCode leftRudder;
    private KeyCode rightRudder;
    private KeyCode reefSail;

    [SerializeField]
    private Movement movement;

    private void Awake()
    {
        Setup();
    }

    private void Update(){
        DetectInput();
    }

    private void DetectInput()
    {
        if (Input.GetKey(increaseSail)){movement.SailStateIncrease();}// Do something
        if (Input.GetKey(leftRudder)){}// Do something
        if (Input.GetKey(rightRudder)){}// Do something
        if (Input.GetKey(reefSail)){movement.SailStateDecrease();}// Do something
    }

    private void Setup()
    {
        // Setup the inputkeycode scriptable object if it's not aleady been done.
        if (inputKeycodes == null){
            inputKeycodes = Resources.Load<InputScriptableObject>("Input/InputFile");
        }
        UpdateInputKeyCodes();

        // Setup GetComponenets if not already attatched in the inspector.
        if (movement == null){movement = GetComponent<Movement>();}
    }

    private void UpdateInputKeyCodes()
    {
        increaseSail = inputKeycodes.IncreaseSailKeycode;
        leftRudder = inputKeycodes.leftRudderKeycode;
        rightRudder = inputKeycodes.RightRudderKeycode;
        reefSail = inputKeycodes.ReefSailKeycode;
    }
}
