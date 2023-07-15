using UnityEngine;
using System.Collections;

namespace PirateGame.Health{

    public class DamageHandler : MonoBehaviour, IProcessDamage
    {
        private IApplyDamage _applyDamage;
        // References to different damageable componenets.
        public void Setup(IApplyDamage applyDamage){
            _applyDamage = applyDamage;
        }

        public void RecieveDamage(DamageAmountStruct[] damageAmounts, DamageAmountStruct[] bonusDamageAmounts = null)
        {
            // Tell the health manager you're taking damage and let it sort out the rest.
           if(_applyDamage == null) return;
           ProcessDamage(damageAmounts);        
           if(bonusDamageAmounts == null) return;
           ProcessDamage(bonusDamageAmounts);
        }

        private void ProcessDamage(params DamageAmountStruct[] damageAmounts)
        {Debug.Log("test");
            foreach(DamageAmountStruct damage in damageAmounts){
                if (damage.GetOverTimeBool){
                    StartCoroutine(DamageTick(damage));
                } else {_applyDamage.ApplyDamageToComponents(damage);}
            }
        }

        private IEnumerator DamageTick(DamageAmountStruct tickDamage){
            float time = tickDamage.GetTime;
            int repeatNum = tickDamage.GetRepeatAmount;
            int currentIteration = 0;

            while(currentIteration < repeatNum){
                _applyDamage.ApplyDamageToComponents(tickDamage);
                yield return new WaitForSeconds(time);
            }
        }
    }

}

