using PirateGame.Health;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : MonoBehaviour, IStructuralDamageModifier, IMobilityDamageModifier, ICorporealDamageModifier, ICannonsLoaded, ITotalCannons, ICurrentAmmoImage
{
    [SerializeField]
    private GameObject _base;

    // Cannons UI data
    private int _leftCannonsLoaded;
    private int _rightCannonsLoaded;
    private int _forwardCannonsLoaded;
    private int _behindCannonsLoaded;
    private int _leftCannonsNumber;
    private int _rightCannonsNumber;
    private int _forwardCannonsNumber;
    private int _behindCannonsNumber;
    // Current Ammo
    private Sprite _currentAmmoImage;
    // Player Health UI data
    private float _structuralHealth;
    private float _mobilityHealth;
    private float _corporealHealth;
    // Base health UI data
    private float _baseStructuralHealth;
    private HealthComponent[] _baseHealthComponenets;

    public void Setup()
    {
        RegisterToTargetHealthComponenets();
    }
    public void CannonsLoaded(CannonPositionEnum pos, int num)
    {
        switch (pos)
        {
            case CannonPositionEnum.Left:
                _leftCannonsLoaded = num;
                break;
            case CannonPositionEnum.Right:
                _rightCannonsLoaded = num;
                break;
            case CannonPositionEnum.Forward:
                _forwardCannonsLoaded = num;
                break;
            case CannonPositionEnum.Behind:
                _behindCannonsLoaded = num;
                break;
        }
    }

    public void TotalCannons(CannonPositionEnum pos, int num)
    {
        switch (pos)
        {
            case CannonPositionEnum.Left:
                _leftCannonsNumber = num;
                break;
            case CannonPositionEnum.Right:
                _rightCannonsNumber = num;
                break;
            case CannonPositionEnum.Forward:
                _forwardCannonsNumber = num;
                break;
            case CannonPositionEnum.Behind:
                _behindCannonsNumber = num;
                break;
        }
    }

    public void CorporealDamageModifier(float modifier, string nameOfSender)
    {
        if (nameOfSender == transform.root.name)
        {
            _corporealHealth = modifier;
        }
    }

    public void MobilityDamageModifier(float modifier, string nameOfSender)
    {
        if (nameOfSender == transform.root.name)
        {
            _mobilityHealth = modifier;
        }
    }

    public void StructuralDamageModifier(float modifier, string nameOfSender)
    {
        if (nameOfSender == transform.root.name)
        {
            _structuralHealth = modifier;
        } else
        {
            _baseStructuralHealth = modifier;
        }
    }
    public void SetUICurrentAmmoImage(Sprite image)
    {
        _currentAmmoImage = image;
    }

    private void OnDestroy()
    {
        UnRegisterToTargetHealthComponenets();
    }
    private void RegisterToTargetHealthComponenets()
    {
        // Search the target for it's health componenets and pass itself into them.
        _baseHealthComponenets = _base.transform.root.GetComponent<HealthManager>().GetHealthComponents().ToArray();
        foreach (HealthComponent componenet in _baseHealthComponenets)
        {
            componenet.AddReciever(this.name, this);
        }
    }
    private void UnRegisterToTargetHealthComponenets()
    {
        if (_baseHealthComponenets == null) return;
        foreach (HealthComponent componenet in _baseHealthComponenets)
        {
            componenet.RemoveReciever(this.name);
        }
    }
}
