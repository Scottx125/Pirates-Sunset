using UnityEngine;
using System.Collections.Generic;

namespace PirateGame.Health{

    public class HealthManager : MonoBehaviour, IApplyDamage
    {
        [SerializeField]
        List<HealthDataStruct> _healthStructList;

        private Dictionary<DamageTypeEnum, List<HealthComponent>> _healthComponenetsDict = new Dictionary<DamageTypeEnum, List<HealthComponent>>();
        
        public void Setup(ICorporealDamageModifier[] corporealDamageModifiers, IStructuralDamageModifier[] structuralDamageModifiers,
        IMobilityDamageModifier[] mobilityDamageModifiers)
        {   
            // Get the objects from the HealthList and set them up with their max health.
            // If the damagetype exists just add the HealthComponent.
            // If it doesn't, create that Dict obj and then add the HealthComponent.
           foreach (HealthDataStruct healthStruct in _healthStructList){
                switch (healthStruct.HealthComponent){
                    case CorporealHealth corp:
                    corp.SetupHealthComponenet(healthStruct.HealthData.MaxHealth, corporealDamageModifiers);
                    break;
                    case StructuralHealth struc:
                        struc.SetupHealthComponenet(healthStruct.HealthData.MaxHealth, structuralDamageModifiers);
                    break;
                    case MobilityHealth mob:
                        mob.SetupHealthComponenet(healthStruct.HealthData.MaxHealth, mobilityDamageModifiers);
                    break;
                    default:
                    // Bleh
                    break;
                }
                foreach (DamageTypeEnum damageType in healthStruct.HealthComponent.GetAssociatedDamageTypes){
                    if (_healthComponenetsDict.ContainsKey(damageType)){
                        _healthComponenetsDict[damageType].Add(healthStruct.HealthComponent);
                    }else{
                        _healthComponenetsDict.Add(damageType, new List<HealthComponent>());
                        _healthComponenetsDict[damageType].Add(healthStruct.HealthComponent);
                    }
                }
           }
        }

        // Identifys the damage type and the applies the damage.
        // Will first see if the income damage is more than 0.
        // Then it will make sure the damage type exists.

        // Then it will go through every HealthObject in that damage type and tell the HealthComponenet
        // to take damage.
        public void ApplyDamageToComponents(DamageAmountStruct damageAmount)
        {
            DamageTypeEnum damageType = damageAmount.GetDamageType;
            int damage = damageAmount.GetDamage;
            if (damage == 0) return;

            if (_healthComponenetsDict.ContainsKey(damageType))
            {
                foreach(HealthComponent healthComponenet in _healthComponenetsDict[damageType]){
                    healthComponenet.TakeDamage(damage);
                }
            }
        }
    }
}