using UnityEngine;
using System.Collections.Generic;

namespace PirateGame.Health{

    public class HealthManager : MonoBehaviour, IApplyDamage
    {
        [SerializeField]
        private SailHealth _sailHealth;
        [SerializeField]
        private HullHealth _hullHealth;
        [SerializeField]
        private CrewHealth _crewHealth;
        [SerializeField]
        private HealthSO _healthData;

        private bool _isSetup;

        private Dictionary<DamageType, List<HealthComponent>> _healthComponenets = new Dictionary<DamageType, List<HealthComponent>>();

        private void Start()
        {
            if (!_isSetup) Setup();
        }

        public void Setup(IDamageModifiers _setDamageModifiers = null)
        {
           _sailHealth.SetupHealthComponenet(_healthData.GetMaxSailHealth, _setDamageModifiers);
           _hullHealth.SetupHealthComponenet(_healthData.GetMaxHullHealth, _setDamageModifiers);
           _crewHealth.SetupHealthComponenet(_healthData.GetMaxCrewHealth, _setDamageModifiers);

           _healthComponenets.Add(DamageType.Hull, new List<HealthComponent>());
           _healthComponenets.Add(DamageType.Sail, new List<HealthComponent>());
           _healthComponenets.Add(DamageType.Crew, new List<HealthComponent>());

           AddHealthComponent(_sailHealth);
           AddHealthComponent(_hullHealth);
           AddHealthComponent(_crewHealth);
           
           _isSetup = true;
        }

        private void AddHealthComponent(HealthComponent componenet)
        {
            foreach(DamageType damageType in componenet.GetAssociatedDamageTypes){
                _healthComponenets[damageType].Add(componenet);
            }
        }

        // Identifys the damage type and the applies the damage.
        // Will first see if the income damage is more than 0.
        // Then it will make sure the damage type has an associated healthComponenet.
        // Then it will go through all the healthComponenets for the associated damage and call TakeDamage.
        public void ApplyDamageToComponents(DamageAmount[] damageAmounts)
        {
            foreach(DamageAmount damageAmount in damageAmounts){
                DamageType damageType = damageAmount.GetDamageType;
                int damage = damageAmount.GetDamage;
                if (damage == 0) continue;

                if (_healthComponenets.TryGetValue(damageType, out List<HealthComponent> healthComponent))
                {
                    foreach(HealthComponent healthComponenet in _healthComponenets[damageType]){
                        healthComponenet.TakeDamage(damage);
                    }
                }
                else 
                {
                    Debug.LogError($"Health componenet for damage type '{damageType}' not found!");
                }
            }
        }
    }
}
