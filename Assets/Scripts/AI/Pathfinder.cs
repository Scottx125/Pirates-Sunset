using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinder : MonoBehaviour
{
    [SerializeField]
    private float _pathUpdateDelay = 1f;
    [SerializeField]
    private float _marchOffset = 5f;
    [SerializeField]
    private float _pathTimeLimit = 2f;
    [SerializeField]
    [Range(0, 1)]
    private float _pathSmoothFactor = .5f;

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
        NavMeshHit hit;
        _runTime = 0f;
        Vector3 targetToPathTo = desiredPosition ?? target.position;

        // Sample the target position and see if we can get to it, if not get the cloest area.
        if (NavMesh.SamplePosition(targetToPathTo, out hit, float.MaxValue, NavMesh.AllAreas))
        {
            targetToPathTo = hit.position;
        } else
        {
            Debug.LogWarningFormat("Target position not on navmesh {0}", targetToPathTo.ToString());
        }

        // Try initial path.
        NavMesh.CalculatePath(transform.position, targetToPathTo, NavMesh.AllAreas, _path);
        // Check to see if the path is complete or invalud and then continue.
        yield return new WaitUntil(() => _path.status == NavMeshPathStatus.PathComplete || _path.status == NavMeshPathStatus.PathInvalid || _path.status == NavMeshPathStatus.PathPartial);

        // If the path failed find a new path.
        if (_path.status == NavMeshPathStatus.PathInvalid)
        {
            Debug.Log("Path failed.");
            if (desiredPosition != null)
            {
                StartCoroutine(MarkTowardsTarget(target, targetToPathTo));
                yield return new WaitUntil(() => _path.status == NavMeshPathStatus.PathComplete);
            }
        }

        // If it created a path, add the current path to the waypoints list.
        BuildWaypoints();

        _currentWaypointIndex = 0;
        _elapsedPathTime = 0f;
        _calculatingPath = null;
        yield break;
    }

    private void BuildWaypoints()
    {
        if (_path.corners.Length == 0) return;
        _waypoints.Add(_path.corners[0]);
        if (_path.corners.Length > 1)
        {
            for (int i = 1; i <= _path.corners.Length - 1; i++)
            {
                Vector3 smoothedPoint = Vector3.Lerp(_path.corners[i - 1], _path.corners[i], _pathSmoothFactor);
                _waypoints.Add(smoothedPoint);
                _waypoints.Add(_path.corners[i]);
            }
        }
    }
#nullable disable
    private IEnumerator MarkTowardsTarget(Transform target, Vector3 targetToPathTo)
    {
        // divide distance by attempts as the march amount.
        int attempts = 10;
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
            attempts--;
            if (attempts <= 0)
            {
                Debug.LogErrorFormat("Failed to march towards target {0}", target.position);
                break;
            }
        }
    }
}
