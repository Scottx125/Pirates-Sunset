using System.Collections.Generic;
using UnityEngine;

namespace PirateGame.Health{

    public class Health : MonoBehaviour, IDamageable
    {
        private IUpdateDamagedModifier[] _iOnDamagedComponenets; 

        private int _maxHullHealth = 100, _hullHealth;
        private int _maxSailHealth = 100, _sailHealth;
        private int _maxCrewHealth = 100, _crewHealth;

        private float ToPercent(int health, int maxHealth) => (float)health / maxHealth;
        private float CalculateDamage(int health, int damage) => health = Mathf.Max(health - damage, 0);

        public void Setup(int maxHullHealth, int maxSailHealth, int maxCrewHealth){
            _maxHullHealth = maxHullHealth;
            _maxSailHealth = maxSailHealth;
            _maxCrewHealth = maxCrewHealth;

            _hullHealth = _maxHullHealth;
            _sailHealth = _maxSailHealth;
            _crewHealth = _maxCrewHealth;
            _iOnDamagedComponenets = GetComponents<IUpdateDamagedModifier>();
        }

        // Identifys the damage type and the applies the damage.
        public void TakeDamage(int damage, AttackTypeEnum type){
            switch (type){
                case AttackTypeEnum.Round_Shot:
                    CalculateDamage(_hullHealth, damage);
                    BroadcastHullDamage();
                    break;
                case AttackTypeEnum.Chain_Shot:
                    CalculateDamage(_sailHealth, damage);
                    BroadcastSailDamage();
                    break;
                case AttackTypeEnum.Grape_Shot:
                    CalculateDamage(_crewHealth, damage);
                    BroadcastCrewDamage();
                    break;
            }
        }

        private void BroadcastHullDamage()
        {
            for (int i = 0; i < _iOnDamagedComponenets.Length; i++)
            {
                _iOnDamagedComponenets[i].UpdateHullDamageModifier(ToPercent(_hullHealth, _maxHullHealth));
            }
        }

        private void BroadcastSailDamage()
        {
            for (int i = 0; i < _iOnDamagedComponenets.Length; i++)
            {
                _iOnDamagedComponenets[i].UpdateSailDamageModifier(ToPercent(_sailHealth, _maxSailHealth));
            }
        }

        private void BroadcastCrewDamage()
        {
            for (int i = 0; i < _iOnDamagedComponenets.Length; i++)
            {
                _iOnDamagedComponenets[i].UpdateCrewDamageModifier(ToPercent(_crewHealth, _maxCrewHealth));
            }
        }

        //public void Heal(int amount);
    }
}
