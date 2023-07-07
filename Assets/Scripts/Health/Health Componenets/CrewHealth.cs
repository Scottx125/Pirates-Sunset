using PirateGame.Health;

public class CrewHealth : HealthComponent
{
    private ICrewDamageModifier _sendCrewDamageModifier;

    public virtual void SetupHealthComponenet(int maxHealth, ICrewDamageModifier crewDamageModifierReciever = null)
    {
        _sendCrewDamageModifier = crewDamageModifierReciever;
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;
    }

    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if(_sendCrewDamageModifier != null) _sendCrewDamageModifier.CrewDamageModifier(ToPercent(_currentHealth, _maxHealth));
    }
}
