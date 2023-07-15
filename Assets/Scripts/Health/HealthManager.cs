using UnityEngine;
using System.Collections.Generic;

namespace PirateGame.Health{

    public class HealthManager : MonoBehaviour, IApplyDamage
    {
        [SerializeField]
        List<HealthDataStruct> _healthStructList;

        private bool _isSetup;

        private Dictionary<DamageTypeEnum, List<HealthComponent>> _healthComponenetsDict = new Dictionary<DamageTypeEnum, List<HealthComponent>>();

        private void Start()
        {
            if (!_isSetup) Setup();
        }

        public void Setup(IDamageModifiers _setDamageModifiers = null)
        {   
            // Get the objects from the HealthList and set them up with their max health.
            // If the damagetype exists just add the HealthComponent.
            // If it doesn't, create that Dict obj and then add the HealthComponent.
           foreach (HealthDataStruct healthStruct in _healthStructList){
                healthStruct.HealthComponent.SetupHealthComponenet(healthStruct.HealthData.MaxHealth);
                foreach (DamageTypeEnum damageType in healthStruct.HealthComponent.GetAssociatedDamageTypes){
                    if (_healthComponenetsDict.ContainsKey(damageType)){
                        _healthComponenetsDict[damageType].Add(healthStruct.HealthComponent);
                    }else{
                        _healthComponenetsDict.Add(damageType, new List<HealthComponent>());
                        _healthComponenetsDict[damageType].Add(healthStruct.HealthComponent);
                    }
                }
           }
           _isSetup = true;
        }

        private void AddHealthComponent(HealthComponent componenet)
        {
            foreach(DamageTypeEnum damageType in componenet.GetAssociatedDamageTypes){
                _healthComponenetsDict[damageType].Add(componenet);
            }
        }

        // Identifys the damage type and the applies the damage.
        // Will first see if the income damage is more than 0.
        // Then it will make sure the damage type exists.

        // Then it will go through every HealthObject in that damage type and tell the HealthComponenet
        // to take damage.
        public void ApplyDamageToComponents(DamageAmountStruct[] damageAmounts)
        {
            foreach(DamageAmountStruct damageAmount in damageAmounts){
                DamageTypeEnum damageType = damageAmount.GetDamageType;
                int damage = damageAmount.GetDamage;
                if (damage == 0) continue;

                if (_healthComponenetsDict.ContainsKey(damageType))
                {
                    foreach(HealthComponent healthComponenet in _healthComponenetsDict[damageType]){
                        healthComponenet.TakeDamage(damage);
                    }
                }
            }
        }
    }
}