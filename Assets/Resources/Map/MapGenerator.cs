using System;
using System.Collections.Generic;
using System.Linq;
using Tools;

namespace Map
{
    public class MapGenerator
    {
        public int stageCount;
        public int pathCount;

        private Map generatedMap;

        private readonly RandomList<Func<int, Level>> createConfig;

        public MapGenerator()
        {
            createConfig = new(new()
            {
                new(.5f, (int stage) => new BattleLevel(stage)),
                new(.25f, (int stage) => new EventLevel(stage)),
                new(.25f, (int stage) => new ShopLevel(stage))
            });
        }

        public Map GenerateMap()
        {
            generatedMap = new Map();

            CreateLevels();
            ConnectLevels();

            return generatedMap;
        }

        private void CreateLevels()
        {
            generatedMap.SetStage(0, new()
            {
                new BattleLevel(1),
                new BattleLevel(1),
                new BattleLevel(1)
            });

            for (int i = 1; i < stageCount; i++)
            {
                HashSet<Level> levels = new();

                for (int j = 0; j < pathCount; j++)
                {
                    levels.Add(createConfig.GetValue().Invoke(i + 1));
                }

                generatedMap.SetStage(i, levels);
            }
        }
        private void ConnectLevels()
        {
            for (int i = 1; i < stageCount; i++)
            {
                List<Level> levels = generatedMap.GetStage(i - 1).ToList();
                foreach (Level level in levels)
                {
                    List<Level> noConnectedLevels = generatedMap.GetStage(i).Where(x => x.connectedLevels.Count <= 0).ToList();
                    level.AddConnection(noConnectedLevels[UnityEngine.Random.Range(0, noConnectedLevels.Count)]);
                }

                List<Level> nextLevels = generatedMap.GetStage(i).ToList();
                levels[UnityEngine.Random.Range(0, levels.Count)].AddConnection(nextLevels[UnityEngine.Random.Range(0, nextLevels.Count)]);
            }
        }
    }
}
