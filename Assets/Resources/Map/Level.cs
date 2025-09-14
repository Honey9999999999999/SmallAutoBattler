using System.Collections.Generic;

namespace Map
{
    public abstract class Level
    {
        public HashSet<Level> connectedLevels;

        public Level(int stage)
        {

        }


        public void AddConnection(Level level)
        {
            connectedLevels.Add(level);
        }
        public virtual void SetLevel()
        {

        }
    }
}
