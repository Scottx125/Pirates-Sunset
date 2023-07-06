using UnityEngine;
using System.Collections.Generic;

namespace PirateGame.Health{

    public abstract class HealthComponent : MonoBehaviour
    {
        protected int _maxHealth = 100;

        protected int _currentHealth;
        [SerializeField]
        private List<DamageType> _associatedDamageTypes = new List<DamageType>();

        public IReadOnlyList<DamageType> GetAssociatedDamageTypes => _associatedDamageTypes;
        protected float ToPercent(int health, int maxHealth) => (float)health / maxHealth;
        private float CalculateDamage(int health, int damage) => health = Mathf.Max(health - damage, 0);

        public void SetupHealthComponenet(int maxHealth)
        {
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;
        }

        public abstract void TakeDamage(int damage);

        //public void Heal(int amount);
    }
}
