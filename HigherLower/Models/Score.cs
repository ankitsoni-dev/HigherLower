using System;

namespace HigherLower.Models
{
    public class Score
    {
        public string PlayerName { get; set; }
        public int FinalScore { get; set; }
        public TimeSpan TimeTaken { get; set; }
    }
}
