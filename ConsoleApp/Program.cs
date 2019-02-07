using System;
using System.Linq;
using WebGame.Domain;

namespace ConsoleApp
{
    class Program
    {
        private IUserRepository userRepo;
        private IGameRepository gameRepo;

        private Program(string[] args)
        {
            userRepo = new InMemoryUserRepository();
            gameRepo = new InMemoryGameRepository();

        }

        public static void Main(string[] args)
        {
            new Program(args).RunMenuLoop();
        }

        private void RunMenuLoop()
        {
            Console.WriteLine("Enter desired number of games in match:");
            
            var game = new GameEntity
            {
                TurnsCount = int.TryParse(Console.ReadLine(), out var gamesCount) ? gamesCount : 5
            };

            var humanUser = userRepo.GetOrCreateUser("Human"); // TODO: get or create
            var aiUser = userRepo.GetOrCreateUser("AI");
            game.AddPlayer(humanUser);
            game.AddPlayer(aiUser);
            var savedGame = gameRepo.Insert(game);
            humanUser.CurrentGameId = savedGame.Id;
            aiUser.CurrentGameId = savedGame.Id;
            userRepo.Update(humanUser);
            userRepo.Update(aiUser);

            RunGameLoop(humanUser.Id);
        }

        private void RunGameLoop(string humanUserId)
        {
            while (true)
            {
                var user = userRepo.GetById(humanUserId);
                var game = gameRepo.GetById(user.CurrentGameId);
                ShowScore(game);
                if (game.IsFinished()) return;
                PlayerDecision? decision = AskHumanDecision(game);
                if (!decision.HasValue) break; // Exit game
                game.SetPlayerDecision(humanUserId, decision.Value);
                var aiPlayer = game.Players.First(p => p.UserId != humanUserId);
                game.SetPlayerDecision(aiPlayer.UserId, PlayerDecision.Rock);
                gameRepo.Update(game);
            }

        }

        private static PlayerDecision? AskHumanDecision(GameEntity game)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Select your next decision:");
                Console.WriteLine("1 - Rock");
                Console.WriteLine("2 - Scissors");
                Console.WriteLine("3 - Paper");
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

    internal class GameService
    {
        private readonly IGameRepository gameRepo;

        public GameService(IGameRepository gameRepo)
        {
            this.gameRepo = gameRepo;
        }

        public void SetPlayerDecision(PlayerDecision decision)
        {

        }
    }

    public class RandomAi
    {
    }
}
