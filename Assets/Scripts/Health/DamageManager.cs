using System;
using UnityEngine;

namespace PirateGame.Health{

    public class DamageManager : MonoBehaviour
    {
        private IDamageable _damageable;
        // References to different damageable componenets.
        public void Setup(IDamageable damageable){
            _damageable = damageable;
            BroadcastDamage();
        }

        private void BroadcastDamage()
        {
            // Tell the health manager you're taking damage and let it sort out the rest.
           if(_damageable == null) return;
            _damageable.TakeDamage(5, AttackTypeEnum.Round_Shot);
        }
    }

}

