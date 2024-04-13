using System;

namespace LD55.Models
{
    public class GameState
    {
        public bool IsPaused { get; set; }
        public DateTime TimeStarted { get; set; } = DateTime.Now;
        public TimeSpan TimeElapsed => DateTime.Now - TimeStarted;
    }
}
