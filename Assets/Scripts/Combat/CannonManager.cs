using System;
using System.Collections.Generic;
using UnityEngine;

public class CannonManager : MonoBehaviour, ICannonManagerLoaded, IFireCannons, ICorporealDamageModifier, IChangeAmmo, IAmmunitionData
{
    [SerializeField]
    private CannonSO _cannonData;
    [SerializeField]
    private List<AmmunitionSO> _ammunitionDataList;
    [SerializeField]
    private List<Cannon> _cannonsList;

    private AmmunitionSO _currentAmmunitionLoaded;
    private int _ammoEnumInt = 0;

    private Dictionary<CannonPositionEnum, List<Cannon>> _cannonDict = new Dictionary<CannonPositionEnum, List<Cannon>>();
    private Dictionary<CannonPositionEnum, int> _cannonDictTotalNumber = new Dictionary<CannonPositionEnum, int>();
    private Dictionary<CannonPositionEnum, int> _cannonDictLoaded = new Dictionary<CannonPositionEnum, int>();
    // add and remove based on inventory enabling. After timer runs out remove from list.
    private Dictionary<AmmunitionTypeEnum, AmmunitionSO> _ammunitionDict = new Dictionary<AmmunitionTypeEnum, AmmunitionSO>();
    private Dictionary<AmmunitionTypeEnum, AmmunitionSO> _boonDamage = new Dictionary<AmmunitionTypeEnum, AmmunitionSO>();

    public void Setup(){
        if (_cannonsList != null){
            foreach(Cannon cannon in _cannonsList){
                cannon.Setup(this, _cannonData);
            }
            AssembleAmmoDict();
            PopulateCannonDict();
            PopulateCannonsTotalAndLoaded();
        }
    }

    private void AssembleAmmoDict()
    {
        foreach(AmmunitionSO so in _ammunitionDataList){
            _ammunitionDict.Add(so.GetAmmunitionType, so);
        }
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

    public AmmunitionSO AmmunitionData(AmmunitionTypeEnum index){
        return _ammunitionDict[index];
    }

    public void ChangeAmmoType(AmmunitionTypeEnum? ammoToLoad, int? iterate){
        // Load ammo by it's passed EnumType
        if (ammoToLoad != null){
            _currentAmmunitionLoaded = _ammunitionDict[(AmmunitionTypeEnum)ammoToLoad];
            _ammoEnumInt = (int)ammoToLoad;
            return;
        }
        // Load ammo based on scroll wheel input.
        if (iterate != null){
            _ammoEnumInt += (int)iterate;
            Mathf.Clamp(_ammoEnumInt, 0, Enum.GetValues(typeof(AmmunitionTypeEnum)).Length - 1);
            AmmunitionTypeEnum ammoToLoadByInt = (AmmunitionTypeEnum)Enum.ToObject(typeof(AmmunitionTypeEnum), _ammoEnumInt);
            _currentAmmunitionLoaded = _ammunitionDict[ammoToLoadByInt];
            return;
        }
    }
    // Fire based on position and if cannons are loaded.
    public void FireCannons(CannonPositionEnum position){
        if (_cannonDict.ContainsKey(position)){
            foreach(Cannon cannon in _cannonDict[position]){
                if (cannon.GetCannonLoaded == true){
                    if (_boonDamage.ContainsKey(_currentAmmunitionLoaded.GetAmmunitionType)){
                        StartCoroutine(cannon.Fire(_currentAmmunitionLoaded, _boonDamage[_currentAmmunitionLoaded.GetAmmunitionType]));
                    } else {StartCoroutine(cannon.Fire(_currentAmmunitionLoaded));}

                    _cannonDictLoaded[position]--;
                }
            }
        }
    }

    public void CannonLoaded(CannonPositionEnum position)
    {
        _cannonDictLoaded[position]++;
    }

    public void CorporealDamageModifier(float modifier)
    {
        foreach (Cannon cannon in _cannonsList){
            cannon.ModifyReloadTime(modifier);
        }
    }
}
