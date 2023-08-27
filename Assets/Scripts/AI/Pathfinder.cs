using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinder : MonoBehaviour
{
    [SerializeField]
    private float _pathUpdateDelay = .5f;
    [SerializeField]
    private float _marchOffset = 5f;
    [SerializeField]
    private float _pathTimeLimit = 2f;

    private List<Vector3> _waypoints = new List<Vector3>();
    private Coroutine _calculatingPath;
    private NavMeshPath _path;
    private float _elapsedPathTime;
    private float _runTime;
    private int _currentWaypointIndex = 0;


    public void Setup()
    {
        _path = new NavMeshPath();
        _elapsedPathTime = 0f;
        _runTime = 0f;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wireframe sphere at the specified position
        Gizmos.color = Color.red; // You can change the color if desired
        if (_waypoints.Count > 0)
        {
            foreach (Vector3 waypoint in _waypoints)
            {
                Gizmos.DrawWireSphere(waypoint, 10f); // 0.5f is the radius of the sphere
            }
        }
        
    }
#nullable enable
    public Vector3? PathToTarget(Transform target, Vector3? desiredPosition)
    {
        if (_elapsedPathTime >= _pathUpdateDelay && _calculatingPath == null)
        {
            _waypoints.Clear();
            _calculatingPath = StartCoroutine(CalculatePath(target, desiredPosition));
        }
        if (_waypoints.Count > 0){
            return _waypoints[_currentWaypointIndex];
        }
        return null;
    }
    public Vector3 CheckNextWaypoint()
    {
        return _waypoints[_currentWaypointIndex];
    }
    #nullable disable
    private void Update(){
        _elapsedPathTime += Time.deltaTime;
        _runTime += Time.deltaTime;
        if (_calculatingPath != null && _runTime >= _pathTimeLimit)
        {
            StopCoroutine(_calculatingPath);
        }
        NextWaypoint();
    }

    private void NextWaypoint()
    {
        if (_waypoints.Count > 0 && _currentWaypointIndex < _waypoints.Count - 1)
        {
            if (Vector3.Distance(transform.position, _waypoints[_currentWaypointIndex]) < 5f)
            {
                _currentWaypointIndex++;
            }
        }
    }
    #nullable enable
    private IEnumerator CalculatePath(Transform target, Vector3? desiredPosition)
    {
        _runTime = 0f;
        Vector3 targetToPathTo = (Vector3)(desiredPosition == null ? target.position : desiredPosition);
        // Try initial path.
        NavMesh.CalculatePath(transform.position, targetToPathTo, NavMesh.AllAreas, _path);
        // Check to see if the path is complete or invalud and then continue.
        yield return new WaitUntil(() => _path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete || _path.status == UnityEngine.AI.NavMeshPathStatus.PathInvalid);
        // If the path failed find a new path.
        if (_path.status == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            Debug.Log("Path failed.");
            if (desiredPosition != null)
            {
                // March the waypoint towards the target until it succeeds or is within 20f of the target.
                while (_path.status != UnityEngine.AI.NavMeshPathStatus.PathComplete && Vector3.Distance(targetToPathTo, target.position) >= 20f)
                {

                    // Get the direction to the target, and march it by the _marchOffset.
                    // If it fails continue looping otherwise break.
                    Vector3 dirToTarget = (target.position - targetToPathTo).normalized;
                    Vector3 offset = dirToTarget * _marchOffset;
                    targetToPathTo += offset;
                    NavMesh.CalculatePath(transform.position, targetToPathTo, NavMesh.AllAreas, _path);
                    yield return new WaitUntil(() => _path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete || _path.status == UnityEngine.AI.NavMeshPathStatus.PathInvalid);
                    if (_path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete)
                    {
                        break;
                    }
                }
            }
        }
        // If it created a path, add the current path to the waypoints list.
        foreach (Vector3 waypoint in _path.corners)
        {
            _waypoints.Add(waypoint);
        }
        _currentWaypointIndex = 0;
        _elapsedPathTime = 0f;
        _calculatingPath = null;
        yield break;
    }
    #nullable disable
}
