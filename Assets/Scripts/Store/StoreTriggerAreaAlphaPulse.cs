using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTriggerAreaAlphaPulse : MonoBehaviour
{

    [SerializeField]
    private Material _transparencyObjectMat;

    [SerializeField]
    [Range(0.1f, 1f)]
    private float _minAlpha = 0.1f, _maxAlpha = 1f;

    [SerializeField]
    private float _pulseCycleTime = 1f;

    private bool _bPulseTargetFlip;

    private Color _transparency;

    private float _velocity = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (_transparencyObjectMat == null)
        {
            _transparencyObjectMat = GetComponent<Material>();
        }
    }

    private void Update()
    {
        Pulse();
    }

    private void Pulse()
    {
        _transparency = _transparencyObjectMat.color;

        if (_bPulseTargetFlip)
        {
            _transparency.a = Mathf.SmoothDamp(_transparency.a, _minAlpha, ref _velocity, _pulseCycleTime);
            if (_transparency.a <= _minAlpha + 0.05f) _bPulseTargetFlip = !_bPulseTargetFlip;
        }
        else
        {
            _transparency.a = Mathf.SmoothDamp(_transparency.a, _maxAlpha, ref _velocity, _pulseCycleTime);
            if (_transparency.a >= _maxAlpha - 0.05f) _bPulseTargetFlip = !_bPulseTargetFlip;
        }

        _transparencyObjectMat.color = _transparency;
    }
}
