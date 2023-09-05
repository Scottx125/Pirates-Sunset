using PirateGame.Control;
using PirateGame.Moving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : DeathAbstract
{

    // Disable AI (make sure to unregister from events/listening )
    // Set speed to 0
    // Play death animation and sink for 10 seconds
    // Destroy the AI once it's out of sight.
    private Coroutine _death;
    private GameObject _aiComponenets;
    private MovementManager _movementManager;


    public void Setup(GameObject aiComponenets, MovementManager movementManager)
    {
        _aiComponenets = aiComponenets;
        _movementManager = movementManager;
    }

    public override IEnumerator OnDeath()
    {
        // Stop input.
        _aiComponenets.SetActive(false);
        // Stop movement.
        _movementManager.ChangeSpeed(SpeedModifierEnum.Reefed_Sails, null);
        _movementManager.TurnLeft(false);
        _movementManager.TurnRight(false);
        // Play death animation and mvoe down.
        StartCoroutine(DeathAnimation());

        yield return new WaitForSeconds(3f);
        Destroy(transform.gameObject);
        yield return null;
    }

    public override IEnumerator DeathAnimation()
    {
        // Explosion
        while (true)
        {
            transform.position += (Vector3.down * 2f) * Time.deltaTime;
            yield return null;
        }
    }

    public override void StructuralDamageModifier(float modifier, string nameOfSender)
    {
        if (modifier <= 0 && _death == null)
        {
            _death = StartCoroutine(OnDeath());
        }
    }
}
