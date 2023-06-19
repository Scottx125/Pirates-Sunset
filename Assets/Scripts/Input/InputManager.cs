using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private InputScriptableObject inputKeycodes;

    private KeyCode increaseSail;
    private KeyCode leftRudder;
    private KeyCode rightRudder;
    private KeyCode reefSail;

    // Implement code that allows the user to change the input values. Only applying them when they hit the ok button.
    // Also add behaviour that checks keycodes when they are entered to prevent conflicts.
}
