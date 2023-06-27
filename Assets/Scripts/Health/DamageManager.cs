using UnityEngine;

namespace PirateGame.Health{
    public class DamageManager : MonoBehaviour, IDamageable
    {
        private Sail _sail;
        private Hull _hull;
        private Crew _crew;
        // References to different damageable componenets.
        public void Start(){
            Health[] componenets = GetComponents<Health>();
            foreach (Health component in componenets){
                switch(component){
                    case Sail sail:
                    _sail = sail;
                    break;
                    case Hull hull:
                    _hull = hull;
                    break;
                    case Crew crew:
                    _crew = crew;
                    break;
                }
            }
            TakeDamage(50, AttackTypeEnum.Chain_Shot);
        }
        
        public void TakeDamage(int damage, AttackTypeEnum type)
        {
            switch (type){
                case AttackTypeEnum.Round_Shot:
                        _hull.TakeDamage(damage, type);
                break;
                case AttackTypeEnum.Chain_Shot:
                        _sail.TakeDamage(damage, type);
                break;
                case AttackTypeEnum.Grape_Shot:
                        _crew.TakeDamage(damage, type);
                break;           
            }
        }
    }
}
