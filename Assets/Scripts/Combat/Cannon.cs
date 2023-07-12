using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private CannonPositionEnum _position;
    [SerializeField]
    private List<GameObject> _projectileGameObj = new List<GameObject>();
    private List<Projectile> _projectile = new List<Projectile>();
    private bool _loaded = true;
    private float _maxFiringDelay;
    private float _minFiringDelay;
    private float _reloadTime;
    private ICannonManagerLoaded _cannonManagerLoaded;
    private int _currentProjectileIndex = 0;

    public bool GetCannonLoaded => _loaded;
    public CannonPositionEnum GetCannonPosition => _position;

    public void Setup(ICannonManagerLoaded cannonManagerLoaded, CannonSO cannonData){
        _cannonManagerLoaded = cannonManagerLoaded;
        _maxFiringDelay = cannonData.GetMaxFireDelayTime;
        _minFiringDelay = cannonData.GetMinFireDelayTime;
        _reloadTime = cannonData.GetReloadTime;

        for(int i = 0; i <= _projectileGameObj.Count - 1; i++){
            _projectileGameObj[i] = Instantiate(_projectileGameObj[i]);
            _projectileGameObj[i].transform.parent = transform;
            _projectile.Add(_projectileGameObj[i].GetComponent<Projectile>());
            Debug.Log(_projectile.Count);
            _projectile[i].Setup(transform);
            _projectileGameObj[i].SetActive(false);
        }
    }

    public IEnumerator Fire(AmmunitionSO coreDamage, AmmunitionSO bonusDamage = null){
        _loaded = false;
        yield return new WaitForSeconds(Random.Range(_minFiringDelay, _maxFiringDelay));
        _projectileGameObj[_currentProjectileIndex].SetActive(true);
        _projectile[_currentProjectileIndex].UpdateProjectile(coreDamage, bonusDamage);
        StartCoroutine(_projectile[_currentProjectileIndex].CalculateFlight());
        CalculateNextProjectileIndex();
        // Play cool firing effect.
        yield return new WaitForSeconds(_reloadTime);
        CannonLoaded();   
    }

    private void CalculateNextProjectileIndex()
    {
        if (_currentProjectileIndex == _projectileGameObj.Count - 1){
            _currentProjectileIndex = 0;
        } else {_currentProjectileIndex++;}
    }

    private void CannonLoaded(){
        _loaded = true;
        _cannonManagerLoaded.CannonLoaded(_position);
    }
}
