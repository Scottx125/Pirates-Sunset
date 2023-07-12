using System;
using System.Collections;
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

    public AmmunitionSO GetCoreDamage => _coreDamage;
    public AmmunitionSO GetBonusDamage => _bonusDamage;

    public void Setup(Transform parent){
        _startPoint = parent.transform.position;
    }

    public void UpdateProjectile(AmmunitionSO coreDamage, AmmunitionSO bonusDamage = null){
        // Setup variables info from coreDamage.
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
        _endPoint = _startPoint + (transform.parent.forward * _maxRange);
        _endPoint += new Vector3(Random.Range(-_angleOffset,_angleOffset),0f,Random.Range(-_rangeOffset,_rangeOffset));
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
        gameObject.SetActive(false);
    }

    private Vector3 QuadraticLerp(Vector3 startPoint, Vector3 highestPoint, Vector3 endPoint, float percentDistCovered)
    {
        Vector3 startToHighPoint = Vector3.Lerp(startPoint, highestPoint, percentDistCovered);
        Vector3 highPointToEnd = Vector3.Lerp(highestPoint, endPoint, percentDistCovered);
        return Vector3.Lerp(startToHighPoint, highPointToEnd, percentDistCovered);
    }

    private void CalculateHighestPoint()
    {
        float normalizedDistance = _highestPointDistance / _maxRange;
        _highestPoint = Vector3.Lerp(_startPoint, _endPoint, normalizedDistance);
        _highestPoint.y += _highestPointOffset;
    }

    // Collision events to disable with terrain, water.
}
