using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

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

        [Serializable]
        internal class Attribute
        {
            public event Action<float> OnChanged;

            public float GeneralValue { get; protected set; }

            public int BaseValue
            {
                get => baseValue;
                set
                {
                    baseValue = Mathf.Max(1, value);
                    Update();                    
                }
            }
            [SerializeField, Min(1)] private int baseValue;
            public int AdditionalValue
            {
                get => additionalValue;
                set
                {
                    additionalValue = Mathf.Max(0, value);
                    Update();
                }
            }
            [SerializeField, Min(0)] private int additionalValue;

            public virtual void Update()
            {
                GeneralValue = (BaseValue + AdditionalValue) / 10f;
                OnChanged?.Invoke(GeneralValue);
            }
        }
        [Serializable]
        internal class Characteristic : Attribute
        {
            public Attribute Attribute
            {
                get => attribute;
                set
                {
                    attribute = value;
                    Update();
                    attribute.OnChanged += (float _) => Update();
                }
            }
            private Attribute attribute;

            public override void Update()
            {
                GeneralValue = (BaseValue + AdditionalValue) * Attribute.GeneralValue;
            }
        }

        
        private Dictionary<StatType, Attribute> attributeMap;

        public StatType MainStat;

        internal Attribute Intellegence => intellegence;
        [SerializeField] private Attribute intellegence;
        internal Attribute Strenght => strenght;
        [SerializeField] private Attribute strenght;
        internal Attribute Agility => agility;
        [SerializeField] private Attribute agility;


        internal Characteristic AttackPower => attackPower;
        [SerializeField] private Characteristic attackPower;
        internal Characteristic AttackPerSec => attackPerSec;
        [SerializeField] private Characteristic attackPerSec;
        internal Characteristic MaxHealth => maxHealth;
        [SerializeField] private Characteristic maxHealth;
        internal Characteristic MaxMana => maxMana;
        [SerializeField] private Characteristic maxMana;
        internal Characteristic HealthRegeneration => healthRegeneration;
        [SerializeField] private Characteristic healthRegeneration;
        internal Characteristic ManaRegeneration => manaRegeneration;
        [SerializeField] private Characteristic manaRegeneration;


        public float TimeBetweenAttacks => 1 / AttackPerSec.GeneralValue;

        public void Initialize()
        {
            attributeMap = new()
            {
                [StatType.Intellegence] = intellegence,
                [StatType.Strenght] = strenght,
                [StatType.Agility] = agility
            };

            MaxMana.Attribute = Intellegence;
            ManaRegeneration.Attribute = Intellegence;

            MaxHealth.Attribute = Strenght;
            HealthRegeneration.Attribute = Strenght;

            AttackPerSec.Attribute = Agility;

            AttackPower.Attribute = attributeMap[MainStat];
        }
    }
}
