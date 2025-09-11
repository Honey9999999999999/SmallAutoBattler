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
            [SerializeField, Min(1)] private int baseValue = 1;
            public int AdditionalValue
            {
                get => additionalValue;
                set
                {
                    additionalValue = value;
                    Update();
                }
            }
            [SerializeField] private int additionalValue;

            public virtual void Update()
            {
                GeneralValue = Mathf.Max(0.1f, (BaseValue + AdditionalValue) / 10f);
                OnChangedInvoke();
            }

            protected void OnChangedInvoke()
            {
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
                GeneralValue = Mathf.Max(0, (BaseValue + AdditionalValue) * Attribute.GeneralValue);
                OnChangedInvoke();
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
        internal Characteristic AttackPerMin => attackPerMin;
        [SerializeField] private Characteristic attackPerMin;
        internal Characteristic MaxHealth => maxHealth;
        [SerializeField] private Characteristic maxHealth;
        internal Characteristic MaxMana => maxMana;
        [SerializeField] private Characteristic maxMana;
        internal Characteristic HealthRegeneration => healthRegeneration;
        [SerializeField] private Characteristic healthRegeneration;
        internal Characteristic ManaRegeneration => manaRegeneration;
        [SerializeField] private Characteristic manaRegeneration;


        public float TimeBetweenAttacks { get; private set; }

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

            AttackPerMin.Attribute = Agility;
            AttackPerMin.OnChanged += (float value) => TimeBetweenAttacks = 1 / (value / 60);

            AttackPower.Attribute = attributeMap[MainStat];

            Intellegence.Update();
            Strenght.Update();
            Agility.Update();
        }
    }
}
