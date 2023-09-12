using PirateGame.Health;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIUIController : MonoBehaviour, IStructuralDamageModifier, IMobilityDamageModifier, ICorporealDamageModifier, IAmmunitionData
{
    [SerializeField]
    private Canvas _aiUICanvas;
    [SerializeField]
    private Image _sailImageUI;
    [SerializeField]
    private Image _crewImageUI;
    [SerializeField]
    private Image _hullImageUI;

    // AI Health UI data
    private float _structuralHealth;
    private float _mobilityHealth;
    private float _corporealHealth;
    // Appear distance stuff.
    private float _appearDistance;
    private Transform _player;

    private void Start()
    {
        _player = FindFirstObjectByType<PlayerManager>().transform;
    }

    private void Update()
    {
        CanvasVisabilityControl();
    }

    private void CanvasVisabilityControl()
    {
        if (_player != null && Vector3.Distance(transform.position, _player.position) <= _appearDistance)
        {
            _aiUICanvas.enabled = true;
        }
        if (_player != null && Vector3.Distance(transform.position, _player.position) > _appearDistance)
        {
            _aiUICanvas.enabled = false;
        }
    }

    public void CorporealDamageModifier(float modifier, string nameOfSender)
    {
        _corporealHealth = modifier;
        _crewImageUI.fillAmount = _corporealHealth;
    }

    public void MobilityDamageModifier(float modifier, string nameOfSender)
    {
        _mobilityHealth = modifier;
        _sailImageUI.fillAmount = _mobilityHealth;
    }

    public void StructuralDamageModifier(float modifier, string nameOfSender)
    {
        _structuralHealth = modifier;
        _hullImageUI.fillAmount = _structuralHealth;
    }

    public void AmmunitionData(Dictionary<AmmunitionTypeEnum, AmmunitionSO> ammoDict)
    {
        foreach(AmmunitionSO ammo in ammoDict.Values)
        {
            if (ammo.GetMaxRange >= _appearDistance)
            {
                _appearDistance = ammo.GetMaxRange;
            }
        }
    }
}
