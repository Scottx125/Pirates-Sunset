using UnityEngine;
using System.Collections.Generic;

namespace PirateGame.Health{

    public abstract class HealthComponent : MonoBehaviour
    {
        protected int _maxHealth = 100;
        protected int _currentHealth;
        protected ICorporealDamageModifier[] _corpoeralModifiers;
        protected IStructuralDamageModifier[] _structuralModifiers;
        protected IMobilityDamageModifier[] _mobilityModifiers;
        
        [SerializeField]
        private List<DamageTypeEnum> _associatedDamageTypes = new List<DamageTypeEnum>();


        public IReadOnlyList<DamageTypeEnum> GetAssociatedDamageTypes => _associatedDamageTypes;
        protected float ToPercent(int health, int maxHealth) => (float)health / maxHealth;
        private float CalculateDamage(int health, int damage) => health = Mathf.Max(health - damage, 0);

        public virtual void SetupHealthComponenet(int maxHealth, ICorporealDamageModifier[] corporealDamageModifiers = null, IStructuralDamageModifier[] structuralDamageModifiers = null,
        IMobilityDamageModifier[] mobilityDamageModifiers = null)
        {
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;
        _corpoeralModifiers = corporealDamageModifiers;
        _structuralModifiers = structuralDamageModifiers;
        _mobilityModifiers = mobilityDamageModifiers;
        }

        public abstract void TakeDamage(int damage);

        //public void Heal(int amount);
    }
}
