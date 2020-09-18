using HigherLower.Constants;
using HigherLower.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HigherLower
{
    public class Game
    {
        public int currentNumber;
        public int oldNumber;
        public int round;
        public int score;
        public string userName;

        FileParser fileParser;
        Stopwatch stopwatch = new Stopwatch();

        public Game(string userName, FileParser fileParser)
        {
            this.userName = userName;
            this.fileParser = fileParser;
        }

        public bool EndGame()
        {
            stopwatch.Stop();
            TimeSpan time = stopwatch.Elapsed;
            Console.WriteLine("Game Over");
            Console.WriteLine($"Your score is { score }");

            if (score == ScoreConstants.MaxScore)
            {
                Console.WriteLine("Congratulations you completed the game");
            }

            Score newScore = new Score()
            {
                PlayerName = userName,
                FinalScore = score,
                TimeTaken = time
            };

            if (HasBeatHighScore(newScore))
            {
                InsertNewHighScore(newScore, time);
            }

            if (score != ScoreConstants.MaxScore)
            {
                return !PromptRestart();
            }

            return true;
        }

        public bool EvaluateResponse(string response)
        { 
            if (response == "h")
            {
                if (oldNumber < currentNumber)
                {
                    score += ScoreConstants.CorrectScore;
                    return true;
                }
            }

            if (response == "l")
            {
                if (oldNumber > currentNumber)
                {
                    score += ScoreConstants.CorrectScore;
                    return true;
                }
            }

            return false;
        }

        public int GetNextNumber()
        {
            Random random = new Random();
            int nextNumber;
            oldNumber = currentNumber;

            do
            {
                nextNumber = random.Next(0, 100);
            } while (nextNumber == currentNumber);

            return nextNumber;
        }

        public string GetUserInput()
        {
            InputParser parser = new InputParser();
            bool validInput;
            string response = string.Empty;

            do
            {
                validInput = parser.ParseScoreResponse(Console.ReadLine(), out string parsedResponse);
                if (!validInput)
                {
                    Console.WriteLine("Your input is invalid. Please state either higher or lower");
                }
                response = parsedResponse;
            }
            while (validInput == false);

            return response;
        }

        public bool HasBeatHighScore(Score newScore)
        {
            List<Score> highScores = fileParser.highScores;

            if (highScores.Count == 3)
            {
                return highScores.Any(score => newScore.FinalScore > score.FinalScore || (newScore.FinalScore == score.FinalScore && score.TimeTaken > newScore.TimeTaken));
            }

            return true;
        }

        public void InsertNewHighScore(Score newScore, TimeSpan time)
        {
            fileParser.AddNewHighScore(newScore);
        }

        public bool NextRound()
        {
            string gameResponse;
            bool response = false;

            round++;

            if (round == 1)
            {
                currentNumber = GetNextNumber();

                Console.WriteLine($"Your first number is { currentNumber }");
                Console.WriteLine("Please state whether you think the next number will be higher (h) or lower (l)");

                gameResponse = GetUserInput();

                currentNumber = GetNextNumber();
                response = ReturnResponse(gameResponse);
            }
            
            if (round != 1)
            {
                Console.WriteLine($"Your next number is { currentNumber }");
                Console.WriteLine("Please state whether you think the next number will be higher (h) or lower (l)");

                gameResponse = GetUserInput();

                currentNumber = GetNextNumber();
                response = ReturnResponse(gameResponse);
            }

            if (response)
            {
                Console.WriteLine($"Your current score is { score }");
            }

            if(!response)
            {
                Console.WriteLine($"Your next number is { currentNumber }");
            }

            return response;
        }

        public void PrintLeaderboard()
        {
            Console.WriteLine($"Quickest to { ScoreConstants.MaxScore } correct answers!");
            foreach (Score score in fileParser.highScores)
            {
                int i = fileParser.highScores.IndexOf(score) + 1;
                Console.WriteLine($"{i}. { score.PlayerName } { score.FinalScore } { score.TimeTaken.TotalSeconds.ToString("F") } seconds");
            }
        }

        public bool PromptRestart()
        {
            string response;
            Console.WriteLine("Would you like to play again? Y/N");
            response = Console.ReadLine().ToLower();

            if (response == "y")
            {
                RestartGame();
                return true;
            }

            return false;
        }

        public void RestartGame()
        {
            round = 0;
            currentNumber = 0;
            score = 0;
            oldNumber = 0;

            stopwatch.Restart();
        }

        public bool ReturnResponse(string userInput)
        {
            if (!EvaluateResponse(userInput))
            {
                Console.WriteLine("Your are incorrect");
                return false;
            }
            else
            {
                Console.WriteLine("You are correct");
                return true;
            }
        }

        public void StartGame()
        {
            stopwatch.Start();
        }
    }
}
