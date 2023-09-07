using PirateGame.Moving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDeath : DeathAbstract
{
    private Coroutine _death;
    public override IEnumerator OnDeath()
    {
        // Play death animation and mvoe down.


        yield return new WaitForSeconds(3f);
        GameManager.GetInstance().GameOver();
        yield return null;
    }

    public override void StructuralDamageModifier(float modifier, string nameOfSender)
    {
        if (modifier <= 0 && _death == null)
        {
            _death = StartCoroutine(OnDeath());
        }
    }
}
