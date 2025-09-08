using System;
using UnityEngine;

namespace AutoBattlers
{
    [Serializable]
    public class EntityStats
    {
        public float AttackPower
        {
            get => attackPower;
            set => Mathf.Max(0, value);
        }
        [SerializeField, Min(0)] private float attackPower;

        public float AttackPerSec
        {
            get => attackPerSec;
            set => Mathf.Max(0, value);
        }
        [SerializeField, Min(0)] private float attackPerSec;

        public float TimeBetweenAttacks => 1 / attackPerSec;
    }
}
