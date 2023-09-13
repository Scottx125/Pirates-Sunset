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
        transform.LookAt(new Vector3(_camera.transform.position.x, transform.position.y, _camera.transform.position.z));
    }
}
