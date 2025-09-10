using System;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBattlers
{
    [Serializable]
    public class EntityStats
    {
        public enum StatType
        {
            Intellegence,
            Strenght,
            Agility
        }

        public EntityStats()
        {
            statGetterMap = new()
            {
                [StatType.Intellegence] = () => Intellegence,
                [StatType.Strenght] = () => Strenght,
                [StatType.Agility] = () => Agility
            };
        }

        public event Action OnStatsChanged;

        private readonly Dictionary<StatType, Func<float>> statGetterMap;

        public StatType MainStat;

        public float Intellegence
        {
            get => intellegence / 10;
            set 
            {
                intellegence = Mathf.Max(1, value);
                OnStatsChanged?.Invoke();
            }
        }
        [SerializeField, Min(0)] private float intellegence;

        public float Strenght
        {
            get => strenght / 10;
            set
            {
                strenght = Mathf.Max(1, value);
                OnStatsChanged?.Invoke();
            }
        }
        [SerializeField, Min(0)] private float strenght;

        public float Agility
        {
            get => agility / 10;
            set
            {
                agility = Mathf.Max(1, value);
                OnStatsChanged?.Invoke();
            }
        }
        [SerializeField, Min(0)] private float agility;

        public float BaseAttackPower
        {
            get => baseAttackPower;
            set => baseAttackPower = Mathf.Max(0, value);
        }
        [SerializeField, Min(0)] private float baseAttackPower;

        public float BaseAttackPerSec
        {
            get => baseAttackPerSec;
            set => baseAttackPerSec = Mathf.Max(0, value);
        }
        [SerializeField, Min(0)] private float baseAttackPerSec;

        public float BaseHealth
        {
            get => baseHealth;
            set => baseHealth = Mathf.Max(0, value);
        }
        [SerializeField, Min(0)] private float baseHealth;
        public float BaseHealthRegeneration
        {
            get => baseHealthRegeneration;
            set => baseHealthRegeneration = Mathf.Max(0, value);
        }
        [SerializeField, Min(0)] private float baseHealthRegeneration;

        public float BaseMana
        {
            get => baseMana;
            set => baseMana = Mathf.Max(0, value);
        }
        [SerializeField, Min(0)] private float baseMana;
        public float BaseManaRegeneration
        {
            get => baseManaRegeneration;
            set => baseManaRegeneration = Mathf.Max(0, value);
        }
        [SerializeField, Min(0)] private float baseManaRegeneration;

        public float MaxHealth => baseHealth * Strenght;
        public float HealthRegeneration => baseHealthRegeneration * Strenght;

        public float MaxMana => baseMana * Intellegence;        
        public float ManaRegeneration => baseManaRegeneration * Intellegence;

        public float AttackPower => BaseAttackPower * statGetterMap[MainStat].Invoke();
        public float TimeBetweenAttacks => 1 / (BaseAttackPerSec * Agility);
    }
}
