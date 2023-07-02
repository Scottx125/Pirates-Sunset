using PirateGame.Health;

public class Hull : Health, IHullDamageModifier
{
    private float _hullDamageModifier = 1f;

    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _hullDamageModifier = ToPercent(_currentHealth, _maxHealth);
    }

        public float GetHullDamageModifier()
    {
        return _hullDamageModifier;
    }
}
