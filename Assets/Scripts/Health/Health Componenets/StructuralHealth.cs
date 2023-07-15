using PirateGame.Health;
public class StructuralHealth : HealthComponent
{
    private IStructuralDamageModifier _sendStructuralDamageModifier;

    public virtual void SetupHealthComponenet(int maxHealth, IStructuralDamageModifier structuralDamageModifierReciever = null)
    {
        _sendStructuralDamageModifier = structuralDamageModifierReciever;
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;
    }

    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if(_sendStructuralDamageModifier != null) _sendStructuralDamageModifier.StructuralDamageModifier(ToPercent(_currentHealth, _maxHealth));
    }

}
