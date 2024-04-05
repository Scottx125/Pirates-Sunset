using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreTriggerTextFloat : MonoBehaviour
{
    [SerializeField]
    private float _offsetY = .5f;
    [SerializeField]
    private float _time = 2f;
    [SerializeField]
    private Canvas _canvas;

    private Vector3 _a;
    private Vector3 _b;
    private Vector3 _target;
    private Vector3 _currentPos;
    private Vector3 _originalPos;
    private Vector3 velocity = Vector3.zero;
    
    // Start is called before the first frame update
    void Awake()
    {
        // Setup.
        _canvas = GetComponentInChildren<Canvas>();
        _originalPos = _canvas.transform.position;

        _a = new Vector3(_originalPos.x, _originalPos.y + _offsetY, _originalPos.z);
        _b = new Vector3(_originalPos.x, _originalPos.y - _offsetY, _originalPos.z);

        // Setup current movement.
        _canvas.transform.position = _b;
        _currentPos = _canvas.transform.position;
        _target = _a;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckDistance();
    }

    private void Move()
    {
        _currentPos = Vector3.SmoothDamp(_currentPos, _target, ref velocity, _time);
        _canvas.transform.position = _currentPos;
    }

    private void CheckDistance()
    {
        if (Vector3.Distance(_currentPos, _a) <= 1f)
        {
            _target = _b;
        } 
        else if (Vector3.Distance(_currentPos, _b) <= 1f)
        {
            _target = _a;
        }
    }
}
