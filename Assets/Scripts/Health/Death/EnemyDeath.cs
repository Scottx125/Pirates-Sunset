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
    [SerializeField]
    private ParticleSystem[] _explosions;
    [SerializeField]
    private AudioSource _explosionSound;
    [SerializeField]
    private float _deathTime = 3f;
    [SerializeField]
    private int _pointsOnDeath = 10;

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
        GameManager.GetInstance().AddShipDestroyed();
        GameManager.GetInstance().AddScore(_pointsOnDeath);
        SpawnManager.GetInstance().ShipDestroyed();
        Destroy(transform.gameObject);
        yield return null;
    }

    public IEnumerator DeathAnimation()
    {
        float timer = 0f;
        // Explosion
        if (_explosions != null)
        {
            foreach (ParticleSystem particle in _explosions)
            {
                particle.Play();
            }
            if (_explosionSound != null)
            {
                _explosionSound.Play();
            }
        }
        while (timer <= _deathTime)
        {
            transform.position += (Vector3.down * 2f) * Time.deltaTime;
            yield return null;
        }
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
