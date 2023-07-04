using UnityEngine;

[CreateAssetMenu(fileName = "Ship", menuName = "ScriptableObjects/Ships", order = 1)]
public class ShipSO : ScriptableObject
{
    [SerializeField]
    private GameObject shipPrefab; 

    [SerializeField]
    private int _maxHullHealth, _maxCrewHealth, _maxSailHealth;

    [SerializeField]
    private float _reloadTime;
    [SerializeField]
    private float _fireDelayTime;
   
    [SerializeField]
    private MoverDataStruct _moverDataStruct;

    public int GetMaxHullHealth => _maxHullHealth;
    public int GetMaxCrewHealth => _maxCrewHealth;
    public int GetMaxSailHealth => _maxSailHealth;

    public float GetReloadTime => _reloadTime;
    public float GetFireDelayTime => _fireDelayTime;

    public MoverDataStruct GetMoverDataStruct => _moverDataStruct;


}
