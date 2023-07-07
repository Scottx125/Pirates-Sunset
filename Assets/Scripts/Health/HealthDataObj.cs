using UnityEngine;
using System;
namespace PirateGame.Health{
    [Serializable]
    public struct HealthDataStruct
    {
        [SerializeField]
        private HealthComponent _healthComponent;
        [SerializeField]
        private HealthSO _healthData;

        public HealthComponent HealthComponent => _healthComponent;
        public HealthSO HealthData => _healthData;
    }
}

