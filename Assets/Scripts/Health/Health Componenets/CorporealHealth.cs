using PirateGame.Health;

public class CorporealHealth : HealthComponent
{
    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if(_corpoeralModifiers != null && _corpoeralModifiers.Length == 0)
        foreach(ICorporealDamageModifier item in _corpoeralModifiers){
            item.CorporealDamageModifier(ToPercent(_currentHealth, _maxHealth));
        }
    }
}
