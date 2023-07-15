using PirateGame.Health;
public class MobilityHealth : HealthComponent
{
    private IMobilityDamageModifier _sendMobilityDamageModifier;

    public virtual void SetupHealthComponenet(int maxHealth, IMobilityDamageModifier mobilityDamageModifierReciever = null)
    {
        _sendMobilityDamageModifier = mobilityDamageModifierReciever;
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;
    }

    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if(_sendMobilityDamageModifier != null) _sendMobilityDamageModifier.MobilityDamageModifier(ToPercent(_currentHealth, _maxHealth));
    }

}
