using PirateGame.Health;
using PirateGame.Helpers;
using UnityEngine;

public class CrewHealth : HealthComponent, IDamageable
{
    private float _crewHealthDamageModifier;

    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _crewHealthDamageModifier = ToPercent(_currentHealth, _maxHealth);
    }
}
