using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private CannonPositionEnum _position;

    private bool _loaded = true;

    private float _maxFiringDelay;
    private float _minFiringDelay;
    private float _reloadTime;
    private ICannonManagerLoaded _cannonManagerLoaded;

    public bool GetCannonLoaded => _loaded;
    public CannonPositionEnum GetCannonPosition => _position;

    public void Setup(ICannonManagerLoaded cannonManagerLoaded, CannonSO cannonData){
        _cannonManagerLoaded = cannonManagerLoaded;
        
        _maxFiringDelay = cannonData.GetMaxFireDelayTime;
        _minFiringDelay = cannonData.GetMinFireDelayTime;
        _reloadTime = cannonData.GetReloadTime;
    }

    public IEnumerator Fire(DamageSO coreDamage, DamageSO bonusDamage = null){
        _loaded = false;
        yield return new WaitForSeconds(Random.Range(_minFiringDelay, _maxFiringDelay));
        // Instantiate object
        // Play cool effect.
        yield return new WaitForSeconds(_reloadTime);
        CannonLoaded();   
    }

    private void CannonLoaded(){
        _loaded = true;
        _cannonManagerLoaded.CannonLoaded(_position);
    }



    
}
