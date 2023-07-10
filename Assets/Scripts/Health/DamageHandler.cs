using System;
using UnityEngine;

namespace PirateGame.Health{

    public class DamageHandler : MonoBehaviour
    {
        private IApplyDamage _applyDamage;
        // References to different damageable componenets.
        public void Setup(IApplyDamage applyDamage){
            _applyDamage = applyDamage;
            // This will be on collision.
            //BroadcastDamage();
        }

        void OnCollisionEnter(Collision other)
        {
            // Get the DamageSO's
            // For each damage object check if it does damage over time.
            // If it doens't call broadcast.
            // If it does utilise a coroutine timer and then call Broadcast for each damage tick.
        }

        private void BroadcastDamage(DamageAmountStruct[] damageAmounts)
        {
            // Tell the health manager you're taking damage and let it sort out the rest.
           if(_applyDamage == null) return;
           _applyDamage.ApplyDamageToComponents(damageAmounts);
        }
    }

}

