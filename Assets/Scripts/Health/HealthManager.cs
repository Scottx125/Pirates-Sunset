using UnityEngine;
namespace PirateGame.Health{
    [RequireComponent(typeof(Sail))]
    [RequireComponent(typeof(Hull))]
    [RequireComponent(typeof(Crew))]
    public class HealthManager : MonoBehaviour, IDamageable
    {
        [SerializeField]
        Sail _sail;
        [SerializeField]
        Hull _hull;
        [SerializeField]
        Crew _crew;

        public void Setup(int crewMaxHealth, int hullMaxHealth, int sailMaxHealth){
            _crew.SetupHealthComponenet(crewMaxHealth);
            _hull.SetupHealthComponenet(hullMaxHealth);
            _sail.SetupHealthComponenet(sailMaxHealth);
        }

        // Identifys the damage type and the applies the damage.
        public void TakeDamage(int damage, AttackTypeEnum type){
            switch (type){
                case AttackTypeEnum.Round_Shot:
                    _hull.TakeDamage(damage);
                    break;
                case AttackTypeEnum.Chain_Shot:
                    _sail.TakeDamage(damage);
                    break;
                case AttackTypeEnum.Grape_Shot:
                    _crew.TakeDamage(damage);
                    break;
            }
        }
    }
}
