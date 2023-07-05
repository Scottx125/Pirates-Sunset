using UnityEngine;
using System.Collections.Generic;
namespace PirateGame.Health{
    public class HealthManager : MonoBehaviour
    {
        [SerializeField]
        SailHealth _sail;
        [SerializeField]
        HullHealth _hull;
        [SerializeField]
        CrewHealth _crew;

        private Dictionary<string, HealthComponent> healthComponenets = new Dictionary<string, HealthComponent>();

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
