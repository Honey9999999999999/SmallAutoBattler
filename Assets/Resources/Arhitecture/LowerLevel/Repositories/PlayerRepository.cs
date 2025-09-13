using System.Collections.Generic;
using System.Linq;
using EntityStatsSystem;
using UnityEngine;

namespace Arhitecture
{
    public class PlayerRepository : Repository
    {
        public class CharacteristicsInfo
        {
            public List<int> values = new();
        }

        private const string PlayerStatsKey = "PlayerStats";

        public EntityStats Stats => stats;
        private EntityStats stats = new();

        public override bool LoadData()
        {
            if (PlayerPrefs.HasKey(PlayerStatsKey))
            {
                CharacteristicsInfo info = JsonUtility.FromJson<CharacteristicsInfo>(PlayerPrefs.GetString(PlayerStatsKey));
                var characteristics = stats.CharacteristicsMap.Values.ToList();

                for (int i = 0; i < characteristics.Count; i++)
                {
                    characteristics[i].BaseValue = info.values[i];
                }

                return true;
            }

            CreatePlayerStats();
            return false;
        }

        public override void SaveData()
        {
            CharacteristicsInfo info = new();

            foreach (var characteristic in stats.CharacteristicsMap.Values)
            {
                info.values.Add(characteristic.BaseValue);
            }

            string serializedObj = JsonUtility.ToJson(info);
            PlayerPrefs.SetString(PlayerStatsKey, serializedObj);
        }

        private void CreatePlayerStats()
        {
            stats = new EntityStats();

            stats.CharacteristicsMap[StatType.Intellegence].BaseValue = 10;
            stats.CharacteristicsMap[StatType.Strenght].BaseValue = 10;
            stats.CharacteristicsMap[StatType.Agility].BaseValue = 10;

            stats.CharacteristicsMap[StatType.MaxHealth].BaseValue = 600;
            stats.CharacteristicsMap[StatType.MaxMana].BaseValue = 200;

            stats.CharacteristicsMap[StatType.AttackPower].BaseValue = 60;
            stats.CharacteristicsMap[StatType.AttackPerMin].BaseValue = 60;
        }

        public override void DeleteData()
        {
            PlayerPrefs.DeleteKey(PlayerStatsKey);
            CreatePlayerStats();
        }
    }
}
