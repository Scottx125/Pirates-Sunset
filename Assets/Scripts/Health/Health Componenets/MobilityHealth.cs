using PirateGame.Health;

public class MobilityHealth : HealthComponent
{
    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if(_mobilityModifiers != null && _mobilityModifiers.Length == 0)
        foreach(IMobilityDamageModifier item in _mobilityModifiers){
            item.MobilityDamageModifier(ToPercent(_currentHealth, _maxHealth));
        }
    }

}
