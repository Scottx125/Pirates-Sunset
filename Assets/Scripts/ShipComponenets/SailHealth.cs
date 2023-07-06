using PirateGame.Health;

public class SailHealth : HealthComponent
{
    private float _sailHealthDamageModifier;

    public override DamageType AssociatedDamageType => DamageType.Sail;

    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _sailHealthDamageModifier = ToPercent(_currentHealth, _maxHealth);
    }

}
