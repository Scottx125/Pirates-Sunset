using UnityEngine;
using System.Collections.Generic;
using System;

namespace PirateGame.Health{
    public class HealthManager : MonoBehaviour
    {
        [SerializeField]
        SailHealth _sail;
        [SerializeField]
        HullHealth _hull;
        [SerializeField]
        CrewHealth _crew;

        private Dictionary<DamageType, HealthComponent> _healthComponenets = new Dictionary<DamageType, HealthComponent>();

        public void Setup(params (HealthComponent healthComponent, int maxHealth)[] healthComponenetData)
        {
            foreach(var (healthComponenet, maxHealth) in healthComponenetData){
                healthComponenet.SetupHealthComponenet(maxHealth);
                AddHealthComponent(healthComponenet);
            }
        }

        private void AddHealthComponent(HealthComponent componenet)
        {
            _healthComponenets[componenet.AssociatedDamageType] = componenet;
        }

        // Identifys the damage type and the applies the damage.
        public void ApplyDamageToComponents(DamageAmount[] damageAmounts)
        {
            foreach(DamageAmount damageAmount in damageAmounts){
                DamageType damageType = damageAmount.GetDamageType;
                int damage = damageAmount.GetDamage;

                if (_healthComponenets.TryGetValue(damageType, out HealthComponent healthComponent))
                {
                    healthComponent.TakeDamage(damage);
                }
                else 
                {
                    Debug.LogError($"Health componenet for damage type '{damageType}' not found!");
                }
            }
        }
    }
}
