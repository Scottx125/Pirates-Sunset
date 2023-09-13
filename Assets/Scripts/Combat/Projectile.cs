using System;
using System.Collections;
using PirateGame.Health;
using UnityEngine;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour
{
    private float _maxRange;
    private float _highestPointDistance;
    private float _rangeOffset;
    private float _angleOffset;
    private float _speed;
    private float _highestPointOffset;
    private float _currentDistance = 0f;
    private AmmunitionSO _coreDamage;
    private AmmunitionSO _bonusDamage;
    private Vector3 _highestPoint;
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private MeshRenderer _meshRenderer;
    private TrailRenderer _trailRenderer;
    private float _disableWaitTime = 1f;
    private Coroutine _calculateFlightCoroutine;

    public void Setup(Transform parent){
        _startPoint = parent.transform.position;
        _meshRenderer = GetComponent<MeshRenderer>();
        _trailRenderer = GetComponentInChildren<TrailRenderer>();
        _disableWaitTime = _trailRenderer.time;
    }

    public void UpdateProjectile(AmmunitionSO coreDamage, AmmunitionSO bonusDamage)
    {
        // Setup variables info from coreDamage.
        _meshRenderer.enabled = true;
        _trailRenderer.enabled = true;
        _coreDamage = coreDamage;
        _bonusDamage = bonusDamage;
        _maxRange = _coreDamage.GetMaxRange;
        _speed = _coreDamage.GetSpeed;
        _rangeOffset = _coreDamage.GetRangeOffset;
        _angleOffset = _coreDamage.GetAngleOffset;
        _highestPointDistance = _coreDamage.GetHighestPointDistance;
        _highestPointOffset = _coreDamage.GetHighestPointOffset;

        // Setup start and end points.
        _currentDistance = 0f;
        _startPoint = transform.parent.position;
        _endPoint = new Vector3(_startPoint.x, 0f, _startPoint.z) + (transform.parent.forward * _maxRange);
        _endPoint += new Vector3(Random.Range(-_angleOffset, _angleOffset), 0f, Random.Range(-_rangeOffset, _rangeOffset));
    }

    public void LaunchProjectile()
    {
        _calculateFlightCoroutine = StartCoroutine(CalculateFlight());
    }

    public IEnumerator CalculateFlight(){
        CalculateHighestPoint();

        float journeyLength = Vector3.Distance(_startPoint, _endPoint);

        while (Vector3.Distance(transform.position, _endPoint) > 0.1f){

            _currentDistance += Time.deltaTime * _speed;
            float percentDistCovered = _currentDistance / journeyLength;

            // calculate position along bezier.
            Vector3 newPos = QuadraticLerp(_startPoint, _highestPoint, _endPoint, percentDistCovered);

            transform.position = newPos;

            yield return null;
        }
        StartCoroutine(WaitBeforeDisable());
    }

    private Vector3 QuadraticLerp(Vector3 startPoint, Vector3 highestPoint, Vector3 endPoint, float percentDistCovered)
    {
        Vector3 startToHighPoint = Vector3.Lerp(startPoint, highestPoint, percentDistCovered);
        Vector3 highPointToEnd = Vector3.Lerp(highestPoint, endPoint, percentDistCovered);
        return Vector3.Lerp(startToHighPoint, highPointToEnd, percentDistCovered);
    }
    
    // Calculate highest point within the QuadraticLerp.
    private void CalculateHighestPoint()
    {
        float normalizedDistance = _highestPointDistance / _maxRange;
        _highestPoint = Vector3.Lerp(_startPoint, _endPoint, normalizedDistance);
        _highestPoint.y += _highestPointOffset;
    }

    // Extract logic out to explain what it does.
    private void OnTriggerEnter(Collider other)
    {
        AmbientCollisions(other);

        // If you hit a target.
        // Might change this if it causes performance issues.
        // Maybe use a queue system if any performance issues.
        TargetCollision(other);
    }

    private static void AmbientCollisions(Collider other)
    {
        // Sort ambient impacts.
        if (other.CompareTag("Sea"))
        {
            // play splash animation and sound
        }
        if (other.CompareTag("Land"))
        {
            // play splash animation and sound
        }
    }

    private void TargetCollision(Collider other)
    {
        IProcessDamage iProcessDmg = other.GetComponent<IProcessDamage>();
        if (iProcessDmg == null) return;

        if (_bonusDamage == null)
        {
            iProcessDmg.RecieveDamage(_coreDamage.GetDamageAmounts);
        }
        else
        {
            iProcessDmg.RecieveDamage(_coreDamage.GetDamageAmounts, _bonusDamage.GetDamageAmounts);
        }
        StartCoroutine(WaitBeforeDisable());
    }

    private IEnumerator WaitBeforeDisable(){
        StopCoroutine(_calculateFlightCoroutine);
        _meshRenderer.enabled = false;
        _trailRenderer.emitting = false;
        yield return new WaitForSeconds(_disableWaitTime);
        _trailRenderer.enabled = false;
        gameObject.SetActive(false);
    }

}
