using HigherLower.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HigherLower
{
    public class FileParser
    {
        public string filePath = "../../../HighScores.txt";
        public List<Score> highScores;

        public void AddNewHighScore(Score newScore)
        {
            highScores.Add(newScore);
            highScores = highScores.OrderBy(score => score.TimeTaken).OrderByDescending(score => score.FinalScore).Take(3).ToList();

            SerializeToFileText();
        }


        public void GetHighScores()
        {
            string line;
            List<Score> scores = new List<Score>();
            using (StreamReader file = new StreamReader(filePath))
            {
                while ((line = file.ReadLine()) != null)
                {
                    string[] score = line.Split(',');
                    Score newScore = new Score()
                    {
                        PlayerName = score[0],
                        FinalScore = Int32.Parse(score[1]),
                        TimeTaken = TimeSpan.Parse(score[2])
                    };

                    scores.Add(newScore);
                }

                scores = scores.OrderBy(score => score.TimeTaken).OrderByDescending(score => score.FinalScore).ToList();

                highScores = scores;
            }                
        }

        public void SerializeToFileText()
        {
            using (TextWriter writer = new StreamWriter(filePath))
            {
                foreach (Score score in highScores)
                {
                    writer.WriteLine(string.Join(", ", score.PlayerName, score.FinalScore, score.TimeTaken.ToString()));
                }
            }
        }
    }
}
