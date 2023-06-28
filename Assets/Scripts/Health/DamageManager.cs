using System;
using UnityEngine;

namespace PirateGame.Health{

    public class DamageManager : MonoBehaviour
    {
        private IDamageable _damageable;
        // References to different damageable componenets.
        private void Start(){
            _damageable = GetComponent<IDamageable>();
            BroadcastDamage();
        }

        private void BroadcastDamage()
        {
            // Also link to a status manager that adds the status to itself async.
            // If it does damage it'll reference Idamageable and deal dmg based on it's attack type.
            // Or if it's a slow effect it'll reference IMoverStatus.
            // Or if it's a repair kit it could reference IHealStatus.
           if(_damageable == null) return;
            _damageable.TakeDamage(5, AttackTypeEnum.Round_Shot);
        }
    }

}

