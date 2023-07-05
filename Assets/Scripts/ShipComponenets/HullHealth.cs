using PirateGame.Health;

public class HullHealth : HealthComponent, IDamageable
{
    private float _hullHealthDamageModifier;

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _hullHealthDamageModifier = ToPercent(_currentHealth, _maxHealth);
    }

}
