using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseMenu : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;
    public void EnableDisable()
    {
        if (_canvas != null)
        {
            _canvas.enabled = !_canvas.enabled;
        }
    }
}
