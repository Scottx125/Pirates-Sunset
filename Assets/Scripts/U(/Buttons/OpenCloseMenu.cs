using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameObject;
    public void EnableDisable()
    {
        if (_gameObject != null)
        {
            _gameObject.SetActive(!_gameObject.activeInHierarchy);   
        }
    }
}
