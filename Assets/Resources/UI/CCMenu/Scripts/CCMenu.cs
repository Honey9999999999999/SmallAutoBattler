using Arhitecture;
using EntityStatsSystem;
using TMPro;
using UnityEngine;
using static EntityStatsSystem.EntityStats;

namespace UI.CCMenu
{
    public class CCMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI availblePointsText;
        [SerializeField] private TextMeshProUGUI statTextPrefab;
        [SerializeField] private StatField statFieldPrefab;
        [SerializeField] private Transform ButtonParent;
        [SerializeField] private Transform StatsParent;

        private EntityStats playerStats;

        public void Initialize()
        {
            playerStats = Game.GetRepository<PlayerRepository>().Stats;

            foreach (var statType in playerStats.CharacteristicsMap.Keys)
            {
                Attribute attribute = playerStats.CharacteristicsMap[statType];

                if (attribute.GetType() == playerStats.CharacteristicsMap[StatType.Intellegence].GetType())
                {
                    StatField statField = Instantiate(statFieldPrefab, ButtonParent);
                    statField.Type = statType;
                }
                else
                {
                    TextMeshProUGUI statText = Instantiate(statTextPrefab, StatsParent);
                    attribute.OnChanged += (float value) => statText.text = $"{statType}: {(value / (int)value == 1 ? value.ToString() : $"{value:F2}")}";
                    float value = attribute.GeneralValue;
                    statText.text = $"{statType}: {(value / (int)value == 1 ? value.ToString() : $"{value:F2}")}";
                }
            }

            StatField.OnAvaiblePointsChanged += (float value) => availblePointsText.text = $"Availble Points: {value}";
        }

        public void StartGame()
        {
            Game.GetRepository<PlayerRepository>().SaveData();
            Game.LoadScene("FOWScene");
        }
    }
}
