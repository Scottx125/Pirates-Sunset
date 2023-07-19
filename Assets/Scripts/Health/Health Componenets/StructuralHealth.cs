using PirateGame.Health;
using UnityEngine;

public class StructuralHealth : HealthComponent
{
    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        Debug.Log(_currentHealth);
        if(_structuralModifiers != null && _structuralModifiers.Length == 0)
        foreach(IStructuralDamageModifier item in _structuralModifiers){
            item.StructuralDamageModifier(ToPercent(_currentHealth, _maxHealth));
        }
    }

}
