using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PirateGame.Helpers
{   
    public static class AIHelpers
    {
        ///<summary>
        ///Returns a Vector3 direction towards a target.
        ///</summary>
        public static Vector3 DirectionToObjective(Vector3 target, Vector3 myTransform)
        {
            return target - myTransform;
        }
        ///<summary>
        ///Returns a List of AmmunitionSO, the first element being the lowest and last being the highest range.
        ///</summary>
        public static List<AmmunitionSO> GetAmmunitionRangesInOrder(List<AmmunitionSO> ammoList)
        {
            ammoList.Sort((x,y) => x.GetMaxRange.CompareTo(y.GetMaxRange));
            return ammoList;
        }
        ///<summary>
        ///Returns the AmmunitionSO which does the top damage of a specified type.
        ///</summary>
        public static AmmunitionSO GetTopDamageOfType(List<AmmunitionSO> ammoList, DamageTypeEnum damageType)
        {
            AmmunitionSO bestAmmo = null;
            // For every ammo in the list.
            foreach (AmmunitionSO ammo in ammoList)
            {
                // And for every DamageStruct held in that ammo.
                foreach(DamageAmountStruct damageStruct in ammo.GetDamageAmounts)
                {
                    // If it's the damage type we're looking for.
                    if (damageStruct.GetDamageType == damageType)
                    {
                        // If nothing has been set, set the current ammo.
                        if (bestAmmo == null)
                        {
                            bestAmmo = ammo;
                            break;
                        }
                        // For each DamageAmountStruct in the ammo we're looking at, compare it against 
                        // the bestAmmo DamageAmountStructs for the type of damage we're looking for.
                        // If it's better than our current bestAmmo, overwrite it.
                        foreach (DamageAmountStruct bestAmmoDamageStruct in bestAmmo.GetDamageAmounts)
                        {
                            if (bestAmmoDamageStruct.GetDamageType == damageType)
                            {
                                bestAmmo = damageStruct.GetDamage > bestAmmoDamageStruct.GetDamage ? ammo : bestAmmo;
                            }
                        }
                    }
                }
            }
            if (bestAmmo == null)
            {
                Debug.LogError("AIHelper GetTopDamageOfType tried to return a NULL value!");
                throw new ArgumentNullException();
            }
            return bestAmmo;
        }
    }
}
