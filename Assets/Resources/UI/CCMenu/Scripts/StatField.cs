using System;
using Arhitecture;
using EntityStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatField : MonoBehaviour
    {
        public static event Action<float> OnAvaiblePointsChanged;
        public event Action<StatType> OnStatChanged;

        private static int availblePoints = 10;

        public StatType Type
        {
            get => type;
            set
            {
                type = value;
                stat = Game.GetRepository<PlayerRepository>().Stats.CharacteristicsMap[Type];
                SetCurrentValue();
            }
        }
        private StatType type;

        [SerializeField] private TextMeshProUGUI currentValue;
        [SerializeField] private Button upButton;
        [SerializeField] private Button downButton;

        private EntityStats.Attribute stat;

        private void Awake()
        {
            Type = StatType.Intellegence;
            upButton.onClick.AddListener(Add);
            downButton.onClick.AddListener(Remove);
        }

        public void Add()
        {
            ChangeAttribute(1);
        }
        public void Remove()
        {
            ChangeAttribute(-1);
        }

        private void SetCurrentValue()
        {
            currentValue.text = $"{Type}: {stat.BaseValue}";
        }

        private void ChangeAttribute(int value)
        {
            if (value < 0 || availblePoints > 0)
            {
                if (stat.BaseValue + value > 0)
                {
                    stat.BaseValue += value;
                    availblePoints -= value;
                }

                SetCurrentValue();

                OnAvaiblePointsChanged?.Invoke(availblePoints);
                OnStatChanged?.Invoke(Type);
            }
        }
    }
}
