using System.Collections.Generic;
using PirateGame.Health;

public class CorporealHealth : HealthComponent
{
    // For this current entitiy.
    private ICorporealDamageModifier[] _corpoeralModifiers;
    // List of enemy states that want to recieve info.
    private Dictionary<string, ICorporealDamageModifier> _recieversDict = new Dictionary<string, ICorporealDamageModifier>();

    public override void AddReciever(string objName, State state)
    {
        if (state is ICorporealDamageModifier modifier)
        {
            _recieversDict.TryAdd(objName, modifier);
        }
    }

    public override void RemoveReciever(string objName)
    {
        _recieversDict.Remove(objName);
    }

    public void SetupHealthComponenet(int maxHealth, ICorporealDamageModifier[] corporealDamageModifiers = null)
    {
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;
        _corpoeralModifiers = corporealDamageModifiers;
    }

    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if(_corpoeralModifiers != null && _corpoeralModifiers.Length == 0)
        foreach(ICorporealDamageModifier item in _corpoeralModifiers){
            item.CorporealDamageModifier(ToPercent(_currentHealth, _maxHealth), transform.root.name);
        }
        foreach(var item in _recieversDict){
            item.Value.CorporealDamageModifier(ToPercent(_currentHealth, _maxHealth), transform.root.name);
        }
    }
}
