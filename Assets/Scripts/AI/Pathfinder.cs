using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField]
    private float _pathUpdateDelay = .5f;

    private List<Vector3> _waypoints = new List<Vector3>();
    private Coroutine _calculatingPath;
    private UnityEngine.AI.NavMeshPath _path;
    private float _elapsedPathTime;
    private int _currentWaypointIndex = 0;


    public void Setup()
    {
        _path = new UnityEngine.AI.NavMeshPath();
        _elapsedPathTime = 0f;
    }

    public Vector3? PathToTarget(Transform target)
    {
        if (_elapsedPathTime >= _pathUpdateDelay && _calculatingPath == null)
        {
            _waypoints.Clear();
            UnityEngine.AI.NavMesh.CalculatePath(transform.position, target.position, UnityEngine.AI.NavMesh.AllAreas, _path);
            _calculatingPath = StartCoroutine(CalculatePath());
        }
        if (_waypoints.Count > 0){
            return _waypoints[_currentWaypointIndex];
        }
        return null;
    }

    private void Update(){
        _elapsedPathTime += Time.deltaTime;
        NextWaypoint();
    }

    private void NextWaypoint()
    {
        if (_waypoints.Count > 0 && _currentWaypointIndex < _waypoints.Count - 1)
        {
            if (Vector3.Distance(transform.position, _waypoints[_currentWaypointIndex]) < .5f)
            {
                _currentWaypointIndex++;
            }
        }
    }

    private IEnumerator CalculatePath()
    {
        // Check to see if the path is complete or invalud and then continue.
        yield return new WaitUntil(()=>_path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete || _path.status == UnityEngine.AI.NavMeshPathStatus.PathInvalid);
        // If the path failed return.
        if (_path.status == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            Debug.Log("Path failed.");
            yield break;
        }
        // If it created a path, add the current path to the waypoints list.
        foreach (Vector3 waypoint in _path.corners)
        {
            _waypoints.Add(waypoint);
        }
        _currentWaypointIndex = 0;
        _elapsedPathTime = 0f;
        _calculatingPath = null;
    }
}
