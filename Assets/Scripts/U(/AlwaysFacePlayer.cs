using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFacePlayer : MonoBehaviour
{

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _camera.transform.position);
    }
}
