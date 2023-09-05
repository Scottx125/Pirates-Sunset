using PirateGame.Health;
using PirateGame.Moving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    [SerializeField]
    private HealthManager _healthManager;
    [SerializeField]
    private DamageHandler _damageHandler;
    [SerializeField]
    private BaseDeath _death;


    private void Awake()
    {
        Setup();
    }
    private void Setup()
    {
        if (_death == null) Debug.LogError("_death is null in BaseManager.");
        if (_healthManager != null)
        {
            ICorporealDamageModifier[] corporealDamageModifiers = {};
            IStructuralDamageModifier[] structuralDamageModifiers = {_death};
            IMobilityDamageModifier[] mobilityDamageModifiers = {};
            _healthManager.Setup(corporealDamageModifiers, structuralDamageModifiers, mobilityDamageModifiers);
        }
        if (_damageHandler != null) _damageHandler.Setup(_healthManager);
    }
}
