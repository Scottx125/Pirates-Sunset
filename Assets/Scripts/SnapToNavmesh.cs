using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;
using UnityEditor;

public class SnapToNavmesh : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Snap();
    }
    [ContextMenu("SnapToMesh")]
    private void Snap()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 100f, -1))
        {
            transform.position = hit.position;
        }
    }
}
