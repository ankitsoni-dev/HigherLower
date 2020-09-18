using HigherLower.Constants;
using System;
using System.IO;

namespace HigherLower
{
    class Program
    {
        public static void Main(string[] args)
        {
            FileParser fileParser = new FileParser();

            if (!File.Exists(fileParser.filePath))
            {
                File.Create(fileParser.filePath).Close();                
            }

            fileParser.GetHighScores();

            Console.WriteLine("Welcome to Higher or Lower!");
            Console.WriteLine("Please enter your name.");

            string playerName = Console.ReadLine();
            Game game = new Game(playerName, fileParser);

            Console.WriteLine($"Your are playing as { playerName }");
            game.StartGame();

            while(game.score != ScoreConstants.MaxScore)
            {
                if (!game.NextRound())
                {
                    if(game.EndGame())
                    {
                        break;
                    }
                }
            }

            if (game.score == ScoreConstants.MaxScore)
            {
                game.EndGame();
            }

            game.PrintLeaderboard();
        }        
    }
}
