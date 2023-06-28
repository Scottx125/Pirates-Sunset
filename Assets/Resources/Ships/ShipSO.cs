using UnityEngine;

[CreateAssetMenu(fileName = "Ship", menuName = "ScriptableObjects/Ships", order = 1)]
public class ShipSO : ScriptableObject
{
    [SerializeField]
    private GameObject shipPrefab; 

    [SerializeField]
    private int _hullHealth, _crewHealth, _sailHealth;
   
    [SerializeField]
    private MoverDataStruct _moverDataStruct;

    public int GetHullHealth => _hullHealth;
    public int GetCrewHealth => _crewHealth;
    public int GetSailHealth => _sailHealth;

    public MoverDataStruct GetMoverDataStruct => _moverDataStruct;


}
