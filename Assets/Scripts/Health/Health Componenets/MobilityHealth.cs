using System.Collections.Generic;
using PirateGame.Health;

public class MobilityHealth : HealthComponent
{
    private IMobilityDamageModifier[] _mobilityModifiers;
    // List of enemy states that want to recieve info.
    private Dictionary<string, IMobilityDamageModifier> _recieversDict = new Dictionary<string, IMobilityDamageModifier>();

    public override void AddReciever(string objName, State state)
    {
        if (state is IMobilityDamageModifier modifier)
        {
            _recieversDict.Add(objName, modifier);
        }
    }

    public override void RemoveReciever(string objName)
    {
        _recieversDict.Remove(objName);
    }

    public void SetupHealthComponenet(int maxHealth, IMobilityDamageModifier[] mobilityDamageModifiers = null)
    {
    _maxHealth = maxHealth;
    _currentHealth = _maxHealth;
    _mobilityModifiers = mobilityDamageModifiers;
    }
    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if(_mobilityModifiers != null && _mobilityModifiers.Length == 0)
        foreach(IMobilityDamageModifier item in _mobilityModifiers){
            item.MobilityDamageModifier(ToPercent(_currentHealth, _maxHealth), transform.root.name);
        }
        foreach(var item in _recieversDict){
            item.Value.MobilityDamageModifier(ToPercent(_currentHealth, _maxHealth), transform.root.name);
        }
    }
}
