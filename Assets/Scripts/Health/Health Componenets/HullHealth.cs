using PirateGame.Health;
public class HullHealth : HealthComponent
{
    private IHullDamageModifier _sendHullDamageModifier;

    public virtual void SetupHealthComponenet(int maxHealth, IHullDamageModifier hullDamageModifierReciever = null)
    {
        _sendHullDamageModifier = hullDamageModifierReciever;
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;
    }

    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if(_sendHullDamageModifier != null) _sendHullDamageModifier.HullDamageModifier(ToPercent(_currentHealth, _maxHealth));
    }

}
