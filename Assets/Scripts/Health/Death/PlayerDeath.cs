using PirateGame.Control;
using PirateGame.Moving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : DeathAbstract
{
    // Disable input
    // Set speed to 0
    // Play death animation and sink for 3 seconds
    // Pause the game and present the ending screen.
    [SerializeField]
    private ParticleSystem[] _explosions;
    [SerializeField]
    private AudioSource _explosionSound;
    [SerializeField]
    private float _deathTime = 3f;
    private Coroutine _death;
    private PlayerInputManager _playerInputManager;
    private MovementManager _movementManager;


    public void Setup(PlayerInputManager playerInputManager, MovementManager movementManager)
    {
        _playerInputManager = playerInputManager;
        _movementManager = movementManager;
    }

    public override IEnumerator OnDeath()
    {
        // Stop input.
        _playerInputManager.SetGameplayInput(false);
        // Stop movement.
        _movementManager.ChangeSpeed(SpeedModifierEnum.Reefed_Sails ,null);
        _movementManager.TurnLeft(false);
        _movementManager.TurnRight(false);
        // Play death animation
        StartCoroutine(DeathAnimation());
        yield return new WaitForSeconds(3f);
        GameManager.GetInstance().GameOver();
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
