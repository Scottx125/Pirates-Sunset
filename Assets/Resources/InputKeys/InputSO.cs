using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Input", menuName = "ScriptableObjects/Input", order = 2)]
public class InputSO : ScriptableObject
{
    [SerializeField]
    private KeyCode _forward = KeyCode.W;
    [SerializeField]
    private KeyCode _back = KeyCode.S;
    [SerializeField]
    private KeyCode _left = KeyCode.A;
    [SerializeField]
    private KeyCode _right = KeyCode.D;

    public KeyCode ForwardKeyCode { get { return _forward; } set { _forward = value; } }
    public KeyCode BackKeyCode { get { return _back; } set { _back = value; } }
    public KeyCode LeftKeyCode { get { return _left; } set { _left = value; } }
    public KeyCode RightKeyCode { get { return _right; } set { _right = value; } }
}
