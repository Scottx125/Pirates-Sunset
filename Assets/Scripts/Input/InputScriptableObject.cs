using UnityEngine;

[CreateAssetMenu(fileName = "InputFile", menuName = "ScriptableObjects/Input", order = 1)]
public class InputScriptableObject : ScriptableObject
{
    [SerializeField]
    // Movement
    private KeyCode increaseSail, leftRudder, rightRudder, reefSail;

    // Movement properties to get and set keycodes.
    public KeyCode IncreaseSailKeycode {get{return increaseSail;} set{increaseSail = value;}}
    public KeyCode leftRudderKeycode {get{return leftRudder;} set{leftRudder = value;}}
    public KeyCode RightRudderKeycode {get{return rightRudder;} set{rightRudder = value;}}
    public KeyCode ReefSailKeycode {get{return reefSail;} set{reefSail = value;}}
}
