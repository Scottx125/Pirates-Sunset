using PirateGame.Health;
public class SailHealth : HealthComponent
{
    private float _sailHealthDamageModifier;

    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _sailHealthDamageModifier = ToPercent(_currentHealth, _maxHealth);
    }

}
