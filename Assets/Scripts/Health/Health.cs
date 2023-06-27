using System.Collections.Generic;
using UnityEngine;

namespace PirateGame.Health{

    public class Health : MonoBehaviour, IDamageable
    {
        private IOnDamaged[] _iOnDamagedComponenets; 

        private int _maxHullHealth = 100, _hullHealth;
        private int _maxSailHealth = 100, _sailHealth;
        private int _maxCrewHealth = 100, _crewHealth;
        public float toPercent(int health, int maxHealth) => (float)health / maxHealth;

        public void Awake(){
            _hullHealth = _maxHullHealth;
            _sailHealth = _maxSailHealth;
            _crewHealth = _maxCrewHealth;
            _iOnDamagedComponenets = GetComponents<IOnDamaged>();
        }

        public void TakeDamage(int damage, AttackTypeEnum type){
            switch (type){
                case AttackTypeEnum.Round_Shot:
                        _hullHealth -= damage;
                        for(int i = 0; i < _iOnDamagedComponenets.Length; i++){
                            _iOnDamagedComponenets[i].OnHullDamage(toPercent(_hullHealth, _maxHullHealth));
                        }
                break;
                case AttackTypeEnum.Chain_Shot:
                        _sailHealth -= damage;
                        for(int i = 0; i < _iOnDamagedComponenets.Length; i++){
                            _iOnDamagedComponenets[i].OnSailDamage(toPercent(_sailHealth, _maxSailHealth));
                        }
                break;
                case AttackTypeEnum.Grape_Shot:
                        _crewHealth -= damage;
                        for(int i = 0; i < _iOnDamagedComponenets.Length; i++){
                            _iOnDamagedComponenets[i].OnCrewDamage(toPercent(_crewHealth, _maxCrewHealth));
                        }
                break;           
            }
        }
        
        //public void Heal(int amount);
    }
}
