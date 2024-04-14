using System;

namespace LD55.Models
{
    [Serializable]
    public class GameState
    {
        public bool IsPaused { get; set; }
        public DateTime TimeStarted { get; set; } = DateTime.Now;
        public TimeSpan TimeElapsed => DateTime.Now - TimeStarted;
        public uint StepCount { get; set; }
        public PlayerPosition PlayerPosition { get; set; } = new PlayerPosition(0, 0);
    }

    public class PlayerPosition
    {
        public PlayerPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public int X { get; set; }
        public int Y { get; set; }
    }
}
