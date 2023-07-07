using PirateGame.Health;
public class SailHealth : HealthComponent
{
    private ISailDamageModifier _sendSailDamageModifier;

    public virtual void SetupHealthComponenet(int maxHealth, ISailDamageModifier sailDamageModifierReciever = null)
    {
        _sendSailDamageModifier = sailDamageModifierReciever;
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;
    }

    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if(_sendSailDamageModifier != null) _sendSailDamageModifier.SailDamageModifier(ToPercent(_currentHealth, _maxHealth));
    }

}
