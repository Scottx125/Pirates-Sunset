using System.Collections.Generic;
using PirateGame.Health;

public class MobilityHealth : HealthComponent
{
    private IMobilityDamageModifier[] _mobilityModifiers;
    // List of enemy states that want to recieve info.
    private Dictionary<string, IMobilityDamageModifier> _recieversDict = new Dictionary<string, IMobilityDamageModifier>();

    public override void AddReciever(string objName, object state)
    {
        if (state is IMobilityDamageModifier modifier)
        {
            _recieversDict.TryAdd(objName, modifier);
        }
        NotifyRecievers();
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
        NotifyRecievers();
    }
    public override void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        NotifyRecievers();
    }

    private void NotifyRecievers()
    {

        foreach (IMobilityDamageModifier item in _mobilityModifiers)
        {
            item.MobilityDamageModifier(ToPercent(_currentHealth, _maxHealth), transform.root.name);
        }
        

        foreach (var item in _recieversDict)
        {
            item.Value.MobilityDamageModifier(ToPercent(_currentHealth, _maxHealth), transform.root.name);
        }
    }
}
