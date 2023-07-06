using PirateGame.Health;

public class CrewHealth : HealthComponent
{
    private float _crewHealthDamageModifier;

    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _crewHealthDamageModifier = ToPercent(_currentHealth, _maxHealth);
    }
}
