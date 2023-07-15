using UnityEngine;
using System.Collections.Generic;

namespace PirateGame.Health{

    public abstract class HealthComponent : MonoBehaviour
    {
        protected int _maxHealth = 100;

        protected int _currentHealth;
        [SerializeField]
        private List<DamageTypeEnum> _associatedDamageTypes = new List<DamageTypeEnum>();

        public IReadOnlyList<DamageTypeEnum> GetAssociatedDamageTypes => _associatedDamageTypes;
        protected float ToPercent(int health, int maxHealth) => (float)health / maxHealth;
        private float CalculateDamage(int health, int damage) => health = Mathf.Max(health - damage, 0);

        public virtual void SetupHealthComponenet(int maxHealth)
        {
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;
        }

        public virtual void TakeDamage(int damage){
            _currentHealth -= damage;
        }

        //public void Heal(int amount);
    }
}
