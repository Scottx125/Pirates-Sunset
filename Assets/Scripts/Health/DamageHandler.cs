using System;
using UnityEngine;

namespace PirateGame.Health{

    public class DamageHandler : MonoBehaviour
    {
        private HealthManager _healthManager;
        // References to different damageable componenets.
        public void Setup(HealthManager healthManager){
            _healthManager = healthManager;
            // This will be on collision.
            BroadcastDamage();
        }

        private void BroadcastDamage(DamageAmount[] damageAmounts)
        {
            // Tell the health manager you're taking damage and let it sort out the rest.
           if(_healthManager == null) return;
           _healthManager.ApplyDamageToComponents(damageAmounts);
        }
    }

}

