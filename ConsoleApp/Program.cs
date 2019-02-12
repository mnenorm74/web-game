using System;
using System.Dynamic;
using System.Linq;
using MongoDB.Driver;
using WebGame.Domain;

namespace ConsoleApp
{
    class Program
    {
        private IUserRepository userRepo;
        private IGameRepository gameRepo;

        private Program(string[] args)
        {
            var mongoConnectionString = Environment.GetEnvironmentVariable("PROJECT5100_MONGO_CONNECTION_STRING")
                                        ?? "mongodb://localhost:27017";
            var db = new MongoClient(mongoConnectionString).GetDatabase("web-game");
            userRepo = new MongoUserRepositoty(db);
            gameRepo = new MongoGameRepository(db);
        }

        public static void Main(string[] args)
        {
            new Program(args).RunMenuLoop();
        }

        private void RunMenuLoop()
        {
            var humanUser = userRepo.ReadOrCreateUser("Human");
            var aiUser = userRepo.ReadOrCreateUser("AI");
            if (FindCurrentGame(humanUser) == null)
                StartNewGame(humanUser, aiUser);
            RunGameLoop(humanUser.Id);

            Console.WriteLine("Game is finished");
            Console.ReadLine();
        }

        private GameEntity StartNewGame(UserEntity humanUser, UserEntity aiUser)
        {
            Console.WriteLine("Enter desired number of turns in game:");
            if (!int.TryParse(Console.ReadLine(), out var turnsCount))
            {
                turnsCount = 5;
                Console.WriteLine($"Bad input. Use default value for turns count: {turnsCount}");
            }
            var game = new GameEntity(turnsCount);
            game.AddPlayer(humanUser);
            game.AddPlayer(aiUser);
            var savedGame = gameRepo.Create(game);
            humanUser.CurrentGameId = savedGame.Id;
            aiUser.CurrentGameId = savedGame.Id;
            userRepo.Update(humanUser);
            userRepo.Update(aiUser);
            return game;
        }

        private GameEntity FindCurrentGame(UserEntity humanUser)
        {
            if (humanUser.CurrentGameId == null) return null;
            var game = gameRepo.ReadById(humanUser.CurrentGameId);
            if (game == null) return null;
            switch (game.Status)
            {
                case GameStatus.WaitingToStart:
                case GameStatus.Playing:
                    return game;
                case GameStatus.Finished:
                case GameStatus.Canceled:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RunGameLoop(string humanUserId)
        {
            while (true)
            {
                var user = userRepo.ReadById(humanUserId);
                var game = gameRepo.ReadById(user.CurrentGameId);
                ShowScore(game);

                if (game.IsFinished())
                {
                    UpdatePlayersWhenGameFinished(game);
                    return;
                }

                PlayerDecision? decision = AskHumanDecision(game);
                if (!decision.HasValue)
                    return;
                game.SetPlayerDecision(humanUserId, decision.Value);

                var aiPlayer = game.Players.First(p => p.UserId != humanUserId);
                game.SetPlayerDecision(aiPlayer.UserId, PlayerDecision.Rock);

                gameRepo.Update(game);
            }
        }

        private void UpdatePlayersWhenGameFinished(GameEntity game)
        {
            foreach (var player in game.Players)
            {
                var playerUser = userRepo.ReadById(player.UserId);
                playerUser.GamesPlayed++;
                playerUser.CurrentGameId = null;
                userRepo.Update(playerUser);
            }
        }

        private static PlayerDecision? AskHumanDecision(GameEntity game)
        {
            Console.WriteLine();
            Console.WriteLine("Select your next decision:");
            Console.WriteLine("1 - Rock");
            Console.WriteLine("2 - Scissors");
            Console.WriteLine("3 - Paper");
            
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.KeyChar == '1') return PlayerDecision.Rock;
                if (key.KeyChar == '2') return PlayerDecision.Scissors;
                if (key.KeyChar == '3') return PlayerDecision.Paper;
                if (key.Key == ConsoleKey.Escape) return null;
            }
        }

        private static void ShowScore(GameEntity game)
        {
            Console.WriteLine();
            Console.WriteLine($"Score: Human {game.Players[0].Score} : {game.Players[1].Score} Computer");
        }
    }
}
