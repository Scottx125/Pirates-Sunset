using PirateGame.Health;

public class CorporealHealth : HealthComponent
{
    private ICorporealDamageModifier _sendCorporealDamageModifier;

    public virtual void SetupHealthComponenet(int maxHealth, ICorporealDamageModifier corporealDamageModifierReciever = null)
    {
        _sendCorporealDamageModifier = corporealDamageModifierReciever;
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;
    }

    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if(_sendCorporealDamageModifier != null) _sendCorporealDamageModifier.CorporealDamageModifier(ToPercent(_currentHealth, _maxHealth));
    }
}
