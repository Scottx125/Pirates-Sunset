using System;
using System.Collections.Generic;
using UnityEngine;

public class CannonManager : MonoBehaviour, ICannonManagerLoaded
{
    [SerializeField]
    private float _loadedRefreshTime = 1f;
    [SerializeField]
    private CannonSO _cannonData;
    [SerializeField]
    private List<DamageSO> _ammunitionDataList;
    [SerializeField]
    private List<Cannon> _cannonsList;

    private DamageSO _currentAmmunitionLoaded;
    private int _currentAmmunitionIndex = 0;

    private Dictionary<CannonPositionEnum, List<Cannon>> _cannonDict = new Dictionary<CannonPositionEnum, List<Cannon>>();
    private Dictionary<CannonPositionEnum, int> _cannonDictTotalNumber = new Dictionary<CannonPositionEnum, int>();
    private Dictionary<CannonPositionEnum, int> _cannonDictLoaded = new Dictionary<CannonPositionEnum, int>();

    public void Setup(){
        if (_cannonsList != null){
            PopulateCannonDict();
            PopulateCannonsTotalAndLoaded();
        }
        // Setup cannons
    }
    // Populated all the cannon totals based on their position.
    private void PopulateCannonsTotalAndLoaded()
    {
        foreach(CannonPositionEnum position in _cannonDict.Keys){
            _cannonDictLoaded.Add(position, _cannonDict[position].Count);
            _cannonDictTotalNumber.Add(position, _cannonDict[position].Count);
        }
    }
    // Populated the main cannon dictionary.
    private void PopulateCannonDict()
    {
        foreach(Cannon cannon in _cannonsList){
            if (_cannonDict.ContainsKey(cannon.GetCannonPosition)){
                _cannonDict[cannon.GetCannonPosition].Add(cannon);
            } else {
                _cannonDict.Add(cannon.GetCannonPosition, new List<Cannon>());
                _cannonDict[cannon.GetCannonPosition].Add(cannon);
            }
        }
    }


    public void ChangeAmmoType(int indexTraverse){
        if (_ammunitionDataList != null){
            Math.Clamp(_currentAmmunitionIndex, 0, _ammunitionDataList.Count -1);
            _currentAmmunitionLoaded = _ammunitionDataList[_currentAmmunitionIndex];
        }
    }

    public void FireCannons(CannonPositionEnum position){
        foreach(Cannon cannon in _cannonDict[position]){
            if (cannon.GetCannonLoaded == true){
                // Fire cannon
                _cannonDictLoaded[position]--;
            }
        }
    }

    public void CannonLoaded(CannonPositionEnum position)
    {
        _cannonDictLoaded[position]++;
    }
}
