using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _camTarget;
    [SerializeField]
    private float _rotationSpeed = .5f;
    [SerializeField]
    private float _heightOffset = 5f;
    [SerializeField]
    private float _minDistToTarget = 5f;
    [SerializeField]
    private float _maxDistToTarget = 10f;

    public void Setup(){
        
    }

    private void LateUpdate()
    {
        transform.position = _camTarget.position - new Vector3(0f, -_heightOffset, _minDistToTarget);
    }


    public void CameraRotation(float xInput, float yInput)
    {
        float xRot = xInput * _rotationSpeed;
        float yRot = yInput * _rotationSpeed;

        transform.RotateAround(_camTarget.position, transform.up, +xRot);
        transform.RotateAround(_camTarget.position, transform.right, -yRot);

        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation.z = 0;
        transform.rotation = Quaternion.Euler(currentRotation);
    }

}
