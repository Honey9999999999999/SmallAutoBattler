using System.Collections.Generic;

namespace Map
{
    public class Map
    {
        public Map()
        {
            if(instance == null)
            {
                instance = this;

                return;
            }
        }

        public static Level CurrentLevel => instance.currentLevel;
        public Level currentLevel;

        public int StageCount => levelMap.Count;
        private readonly Dictionary<int, HashSet<Level>> levelMap;

        private static Map instance;

        public HashSet<Level> GetNextActiveLevels()
        {
            return currentLevel.connectedLevels;
        }

        public HashSet<Level> GetStage(int stage)
        {
            return levelMap[stage];
        }
        public void SetStage(int stage, HashSet<Level> levels)
        {
            levelMap[stage] = levels;
        }
    }
}
