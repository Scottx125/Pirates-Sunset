using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DeathAbstract : MonoBehaviour , IStructuralDamageModifier
{
    public abstract IEnumerator OnDeath();
    public abstract IEnumerator DeathAnimation();
    public abstract void StructuralDamageModifier(float modifier, string nameOfSender);

}
