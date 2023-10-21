using PirateGame.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private GameObject _cannonFireParticles;
    [SerializeField]
    private CannonPositionEnum _position;
    [SerializeField]
    private AudioHandler _audioHandler;
    [SerializeField]
    private List<GameObject> _projectileGameObj = new List<GameObject>();
    private List<Projectile> _projectile = new List<Projectile>();
    private bool _loaded = true;
    private float _maxFiringDelay;
    private float _minFiringDelay;
    private float _maxReloadTime;
    private float _minReloadTime;
    private float _reloadTime;
    private ICannonManagerLoaded _cannonManagerLoaded;
    private int _currentProjectileIndex = 0;

    public bool GetCannonLoaded => _loaded;
    public CannonPositionEnum GetCannonPosition => _position;


    public void Setup(ICannonManagerLoaded cannonManagerLoaded, CannonSO cannonData){
        _cannonManagerLoaded = cannonManagerLoaded;
        _maxFiringDelay = cannonData.GetMaxFireDelayTime;
        _minFiringDelay = cannonData.GetMinFireDelayTime;
        _minReloadTime = cannonData.GetMinReloadTime;
        _maxReloadTime = cannonData.GetMaxReloadTime;
        _reloadTime = _minReloadTime;

        // Load sound setting and set it up.
        if (_audioHandler == null)
        {
            Debug.LogError("Cannon has no instance of AudioHandler.");
            return;
        }

        // Instantiate all assigned projectiles and set them up.
        for(int i = 0; i <= _projectileGameObj.Count - 1; i++){
            _projectileGameObj[i] = Instantiate(_projectileGameObj[i]);
            _projectileGameObj[i].transform.parent = transform;
            _projectile.Add(_projectileGameObj[i].GetComponent<Projectile>());
            _projectile[i].Setup(transform);
            _projectileGameObj[i].SetActive(false);
        }
    }

    // Handle the firing of the cannon as a coroutine.
    public IEnumerator Fire(AmmunitionSO coreDamage, AmmunitionSO bonusDamage = null)
    {
        _loaded = false;
        yield return new WaitForSeconds(Random.Range(_minFiringDelay, _maxFiringDelay));
        // Launch projectile.
        EnableDisableEffects();
        _audioHandler.PlayAudio();
        SetupAndLaunchProjectile(coreDamage, bonusDamage);
        CalculateNextProjectileIndex();
        yield return new WaitForSeconds(_reloadTime);
        //Disable projectile effects and cannon effects.
        EnableDisableEffects();
        CannonLoaded();
    }
    // Modifys the reload time based on the passed health modifier.
    public void ModifyReloadTime(float modifier){
        _reloadTime = Mathf.Clamp(_minReloadTime * (1f + (1f - modifier)), _minReloadTime, _maxReloadTime);
    }

    private void SetupAndLaunchProjectile(AmmunitionSO coreDamage, AmmunitionSO bonusDamage)
    {
        _projectileGameObj[_currentProjectileIndex].SetActive(true);
        _projectile[_currentProjectileIndex].UpdateProjectile(coreDamage, bonusDamage);
        _projectile[_currentProjectileIndex].LaunchProjectile();
    }

    private void EnableDisableEffects()
    {
        _cannonFireParticles.SetActive(!_cannonFireParticles.activeSelf);
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
