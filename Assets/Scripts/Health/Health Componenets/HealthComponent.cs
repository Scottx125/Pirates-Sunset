using UnityEngine;
using System.Collections.Generic;

namespace PirateGame.Health{

    public abstract class HealthComponent : MonoBehaviour, IAddEnemyReciever, IRemoveEnemeyReciever
    {
        public IReadOnlyList<DamageTypeEnum> GetAssociatedDamageTypes => _associatedDamageTypes;

        protected int _maxHealth = 100;
        protected int _currentHealth;
    
        protected float ToPercent(int health, int maxHealth) => (float)health / maxHealth;
        
        [SerializeField]
        private List<DamageTypeEnum> _associatedDamageTypes = new List<DamageTypeEnum>();

        //private float CalculateDamage(int health, int damage) => health = Mathf.Max(health - damage, 0);

        public abstract void TakeDamage(int damage);

        public abstract void AddReciever(string objName, State state);

        public abstract void RemoveReciever(string objName);


        //public void Heal(int amount);
    }
}
