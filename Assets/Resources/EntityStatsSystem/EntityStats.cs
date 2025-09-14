using System;
using System.Collections.Generic;
using UnityEngine;

namespace EntityStatsSystem
{
    [Serializable]
    public class EntityStats
    {
        [Serializable]
        public class Attribute
        {
            public Attribute() : this(10) { }
            public Attribute(int baseValue)
            {
                this.baseValue = baseValue;
            }

            public event Action<float> OnChanged;

            public float GeneralValue { get; protected set; }

            public virtual int BaseValue
            {
                get => baseValue;
                set
                {
                    baseValue = Mathf.Max(1, value);
                    Update();
                }
            }
            [SerializeField, Min(1)] protected int baseValue = 1;
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
        public class Characteristic : Attribute
        {
            public Characteristic() : this(0) { }
            public Characteristic(int baseValue) : base(baseValue) { }

            public override int BaseValue
            {
                get => baseValue;
                set
                {
                    baseValue = Mathf.Max(0, value);
                    Update();
                }
            }

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
                GeneralValue = Mathf.Max(0, (BaseValue * Attribute.GeneralValue) + AdditionalValue);
                OnChangedInvoke();
            }
        }
        [Serializable]
        public class UnattainableCharacteristic : Characteristic
        {
            public UnattainableCharacteristic() : base(1) { }

            public override void Update()
            {
                float value = (BaseValue * (Attribute.GeneralValue + 1)) + AdditionalValue;
                float quadraValue = Mathf.Pow(value, 1.15f);
                GeneralValue = (quadraValue - value) / quadraValue;
                OnChangedInvoke();
            }
        }

        public Dictionary<StatType, Attribute> CharacteristicsMap => characteristicsMap;
        private Dictionary<StatType, Attribute> characteristicsMap;

        public StatType MainStat;

        public EntityStats()
        {
            intellegence = new Attribute();
            strenght = new Attribute();
            agility = new Attribute();

            attackPower = new Characteristic();
            attackPerMin = new Characteristic();

            maxHealth = new Characteristic();
            maxMana = new Characteristic();

            healthRegeneration = new Characteristic(1);
            manaRegeneration = new Characteristic(1);

            armor = new Characteristic(1);

            phisicalResist = new UnattainableCharacteristic();
            magicalResist = new UnattainableCharacteristic();

            Initialize();
        }

        public Attribute Intellegence => intellegence;
        [SerializeField] private Attribute intellegence;
        public Attribute Strenght => strenght;
        [SerializeField] private Attribute strenght;
        public Attribute Agility => agility;
        [SerializeField] private Attribute agility;

        public Characteristic AttackPower => attackPower;
        [SerializeField] private Characteristic attackPower;
        public Characteristic AttackPerMin => attackPerMin;
        [SerializeField] private Characteristic attackPerMin;

        public Characteristic MaxHealth => maxHealth;
        [SerializeField] private Characteristic maxHealth;
        public Characteristic MaxMana => maxMana;
        [SerializeField] private Characteristic maxMana;
        public Characteristic HealthRegeneration => healthRegeneration;
        [SerializeField] private Characteristic healthRegeneration;
        public Characteristic ManaRegeneration => manaRegeneration;
        [SerializeField] private Characteristic manaRegeneration;

        public Characteristic Armor => armor;
        [SerializeField] private Characteristic armor;

        public UnattainableCharacteristic PhisicalResist => phisicalResist;
        [SerializeField] private UnattainableCharacteristic phisicalResist;
        public UnattainableCharacteristic MagicalResist => magicalResist;
        [SerializeField] private UnattainableCharacteristic magicalResist;

        public float TimeBetweenAttacks { get; private set; }

        public void Initialize()
        {
            characteristicsMap = new()
            {
                [StatType.Intellegence] = intellegence,
                [StatType.Strenght] = strenght,
                [StatType.Agility] = agility,
                [StatType.AttackPower] = attackPower,
                [StatType.AttackPerMin] = attackPerMin,
                [StatType.MaxHealth] = maxHealth,
                [StatType.MaxMana] = maxMana,
                [StatType.HealthRegeneration] = healthRegeneration,
                [StatType.ManaRegeneration] = manaRegeneration,
                [StatType.Armor] = armor,
                [StatType.PhisicalResist] = phisicalResist,
                [StatType.MagicalResist] = magicalResist
            };

            maxMana.Attribute = intellegence;
            manaRegeneration.Attribute = intellegence;

            maxHealth.Attribute = strenght;
            healthRegeneration.Attribute = strenght;

            attackPerMin.Attribute = agility;
            attackPerMin.OnChanged += (float value) => TimeBetweenAttacks = 1 / (value / 60);

            armor.Attribute = agility;

            phisicalResist.Attribute = armor;
            magicalResist.Attribute = intellegence;

            attackPower.Attribute = CharacteristicsMap[MainStat];

            intellegence.Update();
            strenght.Update();
            agility.Update();
        }
    }
}
