using System;
using System.Collections;
using PirateGame.Health;
using UnityEngine;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _hullImpactSounds;
    [SerializeField]
    private AudioClip[] _sailImpactSounds;
    [SerializeField]
    private AudioClip[] _crewImpactSounds;
    [SerializeField]
    private AudioClip[] _seaImpactSounds;
    [SerializeField]
    private AudioClip[] _landImpactSounds;

    private float _maxRange;
    private float _highestPointDistance;
    private float _rangeOffset;
    private float _angleOffset;
    private float _speed;
    private float _highestPointOffset;
    private float _currentDistance = 0f;
    private float _disableWaitTime = 0f;
    private AmmunitionSO _coreDamage;
    private AmmunitionSO _bonusDamage;
    private Vector3 _highestPoint;
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private MeshRenderer _meshRenderer;
    private TrailRenderer _trailRenderer;
    private Coroutine _calculateFlightCoroutine;
    private AudioClip _soundToPlay;
    private AudioSource _audioSource;


    public void Setup(Transform parent){
        _startPoint = parent.transform.position;
        _meshRenderer = GetComponent<MeshRenderer>();
        _trailRenderer = GetComponentInChildren<TrailRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void UpdateProjectile(AmmunitionSO coreDamage, AmmunitionSO bonusDamage)
    {
        // Setup variables info from coreDamage.
        _disableWaitTime = _trailRenderer.time;
        _trailRenderer.emitting = true;
        _meshRenderer.enabled = true;
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

    private void AmbientCollisions(Collider other)
    {
        // Sort ambient impacts.
        if (other.CompareTag("Sea"))
        {
            DetermineDisableWaitTime(_seaImpactSounds);
            StartCoroutine(WaitBeforeDisable());
        }
        if (other.CompareTag("Land"))
        {
            DetermineDisableWaitTime(_landImpactSounds);
            StartCoroutine(WaitBeforeDisable());
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
        DetermineDisableWaitTime(DetermineAmmoType());
        StartCoroutine(WaitBeforeDisable());
    }

    private IEnumerator WaitBeforeDisable(){
        StopCoroutine(_calculateFlightCoroutine);
        _trailRenderer.emitting = false;
        _meshRenderer.enabled = false;
        if (_soundToPlay != null)
        {
            _audioSource.PlayOneShot(_soundToPlay);
        }
        yield return new WaitForSeconds(_disableWaitTime);
        
        gameObject.SetActive(false);
    }
    private AudioClip[] DetermineAmmoType()
    {
        // Determine what sound array to play from.
        switch (_coreDamage.GetAmmunitionType)
        {
            case AmmunitionTypeEnum.Round_Shot:
                return _hullImpactSounds;
            case AmmunitionTypeEnum.Chain_Shot:
                return _sailImpactSounds;
            case AmmunitionTypeEnum.Grape_Shot:
                return _crewImpactSounds;
                default:
                return _hullImpactSounds;
        }
    }
    private void DetermineDisableWaitTime(AudioClip[] audioArray)
    {
        // From the chosen array, choose a random sound and then determine if it's length is longer than the trail render.
        // If it is delay death till the sound is finished.
        int selection = Random.Range(0, audioArray.Length);
        _soundToPlay = audioArray[selection];

        if (_soundToPlay.length > _trailRenderer.time) _disableWaitTime = _soundToPlay.length;
        else { _disableWaitTime = _trailRenderer.time; }
    }

}
