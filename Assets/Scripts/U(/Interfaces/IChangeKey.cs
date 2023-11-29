
using UnityEngine;

public interface IChangeKey
{
    public KeyCode GetTempKeyCode();
    public void ResetTempKeyCode();
    public void SetTempKeyCode(KeyCode value);

}
