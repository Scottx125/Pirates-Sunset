using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private CannonPositionEnum _position;
    [SerializeField]
    private GameObject _cannonBall;
    [SerializeField]
    private GameObject _chainShot;
    [SerializeField]
    private GameObject _grapeShot;

    private bool _loaded = true;

    private float _firingDelay;
    private float _reloadTime;
    private ICannonManagerLoaded _cannonManagerLoaded;

    public bool GetCannonLoaded => _loaded;
    public CannonPositionEnum GetCannonPosition => _position;

    public void Setup(ICannonManagerLoaded cannonManagerLoaded){
        _cannonManagerLoaded = cannonManagerLoaded;
        // load cannon data from SO
    }

    public void Fire(DamageType ammoType){
        // Fire after X period of time and prevent firing till reloaded. Might need to be coroutine.
    }
    // FireCoroutine

    private void CannonLoaded(){
        _loaded = true;
        _cannonManagerLoaded.CannonLoaded(_position);
    }



    
}
