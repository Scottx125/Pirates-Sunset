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

    private List<Vector3> _waypoints = new List<Vector3>();
    private Coroutine _calculatingPath;
    private NavMeshPath _path;
    private float _elapsedPathTime;
    private int _currentWaypointIndex = 0;


    public void Setup()
    {
        _path = new NavMeshPath();
        _elapsedPathTime = 0f;
    }

    public Vector3? PathToTarget(Transform target, Transform? desiredPosition)
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

    private IEnumerator CalculatePath(Transform target, Transform? desiredPosition)
    {
        Transform targetToPathTo = desiredPosition == null ? target : desiredPosition;
        // Try initial path.
        NavMesh.CalculatePath(transform.position, targetToPathTo.position, NavMesh.AllAreas, _path);
        // Check to see if the path is complete or invalud and then continue.
        yield return new WaitUntil(() => _path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete || _path.status == UnityEngine.AI.NavMeshPathStatus.PathInvalid);
        // If the path failed find a new path.
        if (_path.status == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            Debug.Log("Path failed.");
            if (desiredPosition != null)
            {
                // March the waypoint towards the target until it succeeds or is within 20f of the target.
                while (_path.status != UnityEngine.AI.NavMeshPathStatus.PathComplete && Vector3.Distance(targetToPathTo.position, target.position) >= 20f)
                {
                    Vector3 dirToTarget = target.position - targetToPathTo.position;
                    Vector3 offset = dirToTarget.normalized * _marchOffset;
                    targetToPathTo.position += offset;
                    NavMesh.CalculatePath(transform.position, targetToPathTo.position, NavMesh.AllAreas, _path);
                }
            }
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
