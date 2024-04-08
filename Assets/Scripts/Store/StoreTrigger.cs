using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject _store;

    BoxCollider _boxCollider;

    void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pirates")
        {
            _store.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Pirates" && _store.activeSelf == true)
        {
            _store.SetActive(false);
        }
    }
}
