using PirateGame.Health;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUIController : MonoBehaviour, IStructuralDamageModifier, IMobilityDamageModifier, ICorporealDamageModifier
{
    // AI Health UI data
    private float _structuralHealth;
    private float _mobilityHealth;
    private float _corporealHealth;

    public void CorporealDamageModifier(float modifier, string nameOfSender)
    {
        _corporealHealth = modifier;
    }

    public void MobilityDamageModifier(float modifier, string nameOfSender)
    {
        _mobilityHealth = modifier;
    }

    public void StructuralDamageModifier(float modifier, string nameOfSender)
    {
        _structuralHealth = modifier;
    }

}
