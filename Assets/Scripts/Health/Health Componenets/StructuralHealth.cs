using System.Collections.Generic;
using PirateGame.Health;
using UnityEngine;

public class StructuralHealth : HealthComponent
{
    private IStructuralDamageModifier[] _structuralModifiers;
    // List of enemy states that want to recieve info.
    private Dictionary<string, IStructuralDamageModifier> _recieversDict = new Dictionary<string, IStructuralDamageModifier>();

    public override void AddReciever(string objName, State state)
    {
        if (state is IStructuralDamageModifier modifier)
        {
            _recieversDict.TryAdd(objName, modifier);
        }
    }

    public override void RemoveReciever(string objName)
    {
        _recieversDict.Remove(objName);
    }

    public void SetupHealthComponenet(int maxHealth, IStructuralDamageModifier[] structuralDamageModifiers = null)
    {
    _maxHealth = maxHealth;
    _currentHealth = _maxHealth;
    _structuralModifiers = structuralDamageModifiers;
    }

    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        Debug.Log(_currentHealth);
        if(_structuralModifiers != null && _structuralModifiers.Length == 0)
        foreach(IStructuralDamageModifier item in _structuralModifiers){
            item.StructuralDamageModifier(ToPercent(_currentHealth, _maxHealth), transform.root.name);
        }
        foreach(var item in _recieversDict){
            item.Value.StructuralDamageModifier(ToPercent(_currentHealth, _maxHealth), transform.root.name);
        }
    }

}
