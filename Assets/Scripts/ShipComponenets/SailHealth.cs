using PirateGame.Health;

public class SailHealth : HealthComponent, IDamageable
{
     private float _sailHealthDamageModifier;

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _sailHealthDamageModifier = ToPercent(_currentHealth, _maxHealth);
    }

}
