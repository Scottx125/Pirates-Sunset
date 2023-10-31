using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    // Include in any object that has data that needs to be saved.
    object CaptureState();
    void RestoreState(object state);
}
