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
        _startPoint = transform.parent.position;
        _endPoint = _startPoint + (transform.parent.forward * _maxRange);
        _endPoint += new Vector3(Random.Range(-_angleOffset,_angleOffset),0f,Random.Range(-_rangeOffset,_rangeOffset));
    }
    public IEnumerator CalculateFlight(){

        CalculateHighestPoint();

        float startTime = Time.time;
        float journeyLength = Vector3.Distance(_startPoint, _endPoint);

        while (Vector3.Distance(transform.position, _endPoint) > .1f){
            float distCovered = (Time.time - startTime) * _speed;
            float percentDistCovered = distCovered / journeyLength;

            // calculate position along bezier.
            Vector3 newPos = QuadraticBezier(_startPoint, _highestPoint, _endPoint, percentDistCovered);

            transform.position = newPos;

            yield return null;
        }
        gameObject.SetActive(false);
    }

    private Vector3 QuadraticBezier(Vector3 startPoint, Vector3 highestPoint, Vector3 endPoint, float percentDistCovered)
    {
        float u = 1 - percentDistCovered;
        float uu = u*u;
        float tt = percentDistCovered * percentDistCovered;

        Vector3 position = uu * startPoint;
        position += 2 * u * percentDistCovered * highestPoint;
        position += tt * endPoint;
        return position;
    }

    private void CalculateHighestPoint()
    {
        float normalizedDistance = _highestPointDistance / _maxRange;
        _highestPoint = Vector3.Lerp(_startPoint, _endPoint, normalizedDistance);
        _highestPoint.y += _highestPointOffset;
    }

    // Collision events to disable with terrain, water.
}
