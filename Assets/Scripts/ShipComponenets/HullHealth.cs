using PirateGame.Health;
public class HullHealth : HealthComponent
{
    private float _hullHealthDamageModifier;

    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _hullHealthDamageModifier = ToPercent(_currentHealth, _maxHealth);
    }

}
