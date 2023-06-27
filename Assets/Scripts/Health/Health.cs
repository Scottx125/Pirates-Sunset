using System.Collections.Generic;
using UnityEngine;

namespace PirateGame.Health{

    public abstract class Health : MonoBehaviour, ISubjectHealth, IDamageable
    {
        private List<IObserverShipMovement> _observers = new List<IObserverShipMovement>();

        private int _maxHealth = 100;
        private int _health;
        public float toPercent => (float)_health / _maxHealth;
        public float getHealth => _health;

        public void Start(){
            _health = _maxHealth;
        }

        public void TakeDamage(int damage, AttackTypeEnum type){
            _health -= damage;
            NotifyOnDamage(toPercent, type);
        }
        
        //public void Heal(int amount);

        public void AddHealthObserver(IObserverShipMovement observer)
        {
            _observers.Add(observer);
        }

         public void RemoveHealthObserver(IObserverShipMovement observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyOnDamage(float percent, AttackTypeEnum type)
        {
            foreach (var observer in _observers){
                observer.OnDamageNotify(percent, type);
            }
        }
    }
}
