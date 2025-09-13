using System;
using UnityEngine;

namespace AutoBattlers
{
    public class Attack
    {
        public Attack(AutoBattler target)
        {
            target.Health.OnGetted += (float value) => OnEnemyTakenDamage?.Invoke(target, value);
            OnEnemyTakenDamage += (AutoBattler target, float value) => UnSub(target);
        }

        public event Action<AutoBattler, float> OnEnemyTakenDamage;

        public DamageType DamageType { get; set; }

        public float Damage
        {
            get => damage;
            set => damage = Mathf.Max(0, value);
        }
        private float damage;

        private void UnSub(AutoBattler target)
        {
            target.Health.OnGetted -= (float value) => OnEnemyTakenDamage?.Invoke(target, value);
            OnEnemyTakenDamage = null;
        }
    }
}
