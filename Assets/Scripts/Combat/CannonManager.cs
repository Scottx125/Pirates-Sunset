using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class CannonManager : MonoBehaviour, ICannonManagerLoaded, IFireCannons, ICorporealDamageModifier, IChangeAmmo
{
    [SerializeField]
    private CannonSO _cannonData;
    [SerializeField]
    private List<AmmunitionSO> _ammunitionDataList;
    [SerializeField]
    private List<Cannon> _cannonsList;

    private AmmunitionSO _currentAmmunitionLoaded;
    private int _ammoEnumInt = 0;

    private ICannonsLoaded _cannonsLoadedUI;
    private ITotalCannons _totalCannonsUI;
    private ICurrentAmmoImage _currentAmmoImage;
    private Dictionary<CannonPositionEnum, List<Cannon>> _cannonDict = new Dictionary<CannonPositionEnum, List<Cannon>>();
    private Dictionary<CannonPositionEnum, int> _cannonDictTotalNumber = new Dictionary<CannonPositionEnum, int>();
    private Dictionary<CannonPositionEnum, int> _cannonDictLoaded = new Dictionary<CannonPositionEnum, int>();
    // add and remove based on inventory enabling. After timer runs out remove from list.
    private Dictionary<AmmunitionTypeEnum, AmmunitionSO> _ammunitionDict = new Dictionary<AmmunitionTypeEnum, AmmunitionSO>();
    //private Dictionary<AmmunitionTypeEnum, AmmunitionSO> _boonDamage = new Dictionary<AmmunitionTypeEnum, AmmunitionSO>();
    #nullable enable
    public void Setup(IAmmunitionData? requiresAmmoData, ICannonsLoaded? cannonsLoadedUI, ITotalCannons? cannonsTotalUI, ICurrentAmmoImage? currentAmmoImage){
        // Set everything up
        if (_cannonsList != null){
            // Set cannons up
            foreach(Cannon cannon in _cannonsList){
                cannon.Setup(this, _cannonData);
            }
            // set ammo and cannon dicts up.
            AssembleAmmoDict();
            if (_ammunitionDict.Count != Enum.GetValues(typeof(AmmunitionTypeEnum)).Length)
            {
                Debug.LogError("Ammo on {0} is not filled properly!", this);
            }
            if (requiresAmmoData != null) SendAmmoData(requiresAmmoData);
            PopulateCannonDict();
            PopulateCannonsTotalAndLoaded();
            // Ensure we've got the first ammo type loaded.
            _currentAmmunitionLoaded = _ammunitionDict.First().Value;
        } else {
            Debug.Log("Error, no cannons have been set up!");
        }
        // Add UI requesters to cannons UI.
        if (cannonsLoadedUI != null)
        {
            _cannonsLoadedUI = cannonsLoadedUI;
            foreach(CannonPositionEnum pos in _cannonDictLoaded.Keys)
            {
                NotifyLoadedCannons(pos);
            }
        }
        if (cannonsTotalUI != null)
        {
            _totalCannonsUI = cannonsTotalUI;
            foreach(KeyValuePair<CannonPositionEnum, int> kvp in _cannonDictTotalNumber)
            {
                _totalCannonsUI.TotalCannons(kvp.Key, kvp.Value);
            }
        }
        if (currentAmmoImage != null)
        {
            _currentAmmoImage = currentAmmoImage;
            _currentAmmoImage.SetUICurrentAmmoImage(_currentAmmunitionLoaded.GetAmmoImage);
        }
    }
    #nullable disable
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
    // Sends the cannon dict off to the object that needs it (AI/UI).
    private void SendAmmoData(IAmmunitionData data){
        data.AmmunitionData(_ammunitionDict);
    }

    public void ChangeAmmoType(AmmunitionTypeEnum? ammoToLoad, bool? iterate){
        // Load ammo by it's passed EnumType
        if (ammoToLoad != null && _ammunitionDict.ContainsKey((AmmunitionTypeEnum)ammoToLoad)){
            _currentAmmunitionLoaded = _ammunitionDict[(AmmunitionTypeEnum)ammoToLoad];
            _ammoEnumInt = (int)ammoToLoad;
            return;
        }
        
        // Load ammo based on scroll wheel input.
        if (iterate != null){
            // Iterate and then clamp.
            if ((bool)iterate)
            {
                _ammoEnumInt += 1;
            } else
            {
                _ammoEnumInt -= 1;
            }
            Mathf.Clamp(_ammoEnumInt, 0, Enum.GetValues(typeof(AmmunitionTypeEnum)).Length - 1);
            // Get the enum based on the value iterated.
            AmmunitionTypeEnum ammoToLoadIntToEnum = (AmmunitionTypeEnum)Enum.ToObject(typeof(AmmunitionTypeEnum), _ammoEnumInt);
            // If the enum is in the dict set the new ammo type.
            if (_ammunitionDict.ContainsKey(ammoToLoadIntToEnum)){
                _currentAmmunitionLoaded = _ammunitionDict[ammoToLoadIntToEnum];
                // Make sure to update ammo image for UI's.
                _currentAmmoImage.SetUICurrentAmmoImage(_currentAmmunitionLoaded.GetAmmoImage);
            }
            return;
        }
    }
    // Fire based on position and if cannons are loaded.
    public void FireCannons(CannonPositionEnum position){
        if (_cannonDict.ContainsKey(position)){
            foreach(Cannon cannon in _cannonDict[position]){
                if (cannon.GetCannonLoaded == true){
                    //if (_boonDamage.ContainsKey(_currentAmmunitionLoaded.GetAmmunitionType)){
                    //    StartCoroutine(cannon.Fire(_currentAmmunitionLoaded, _boonDamage[_currentAmmunitionLoaded.GetAmmunitionType]));
                    //} else {}
                    StartCoroutine(cannon.Fire(_currentAmmunitionLoaded));
                    _cannonDictLoaded[position]--;
                }
            }
            NotifyLoadedCannons(position);
        }
    }

    public void CannonLoaded(CannonPositionEnum position)
    {
        _cannonDictLoaded[position]++;
        NotifyLoadedCannons(position);
    }

    public void CorporealDamageModifier(float modifier, string nameOfSender)
    {
        foreach (Cannon cannon in _cannonsList){
            cannon.ModifyReloadTime(modifier);
        }
    }

    public void NotifyLoadedCannons(CannonPositionEnum pos)
    {
        _cannonsLoadedUI.CannonsLoaded(pos, _cannonDictLoaded[pos]);
    }
}
