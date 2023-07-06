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

        private void BroadcastDamage(DamageAmount[] damageAmounts)
        {
            // Tell the health manager you're taking damage and let it sort out the rest.
           if(_applyDamage == null) return;
           _applyDamage.ApplyDamageToComponents(damageAmounts);
        }
    }

}

