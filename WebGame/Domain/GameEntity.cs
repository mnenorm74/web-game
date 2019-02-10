using System;
using System.Collections.Generic;
using System.Linq;

namespace WebGame.Domain
{
    public class GameEntity
    {
        public GameEntity()
        {
            Players = new List<Player>();
            Status = GameStatus.WaitingToStart;
        }

        public GameEntity(string id)
        {
            Id = id;
            Players = new List<Player>();
            Status = GameStatus.WaitingToStart;
        }

        public string Id;
        public List<Player> Players;
        public int TurnsCount;
        public int CurrentTurnIndex;
        public GameStatus Status;

        public void AddPlayer(UserEntity user)
        {
            if (Status != GameStatus.WaitingToStart)
                throw new ArgumentException(Status.ToString());
            Players.Add(new Player(user.Id, user.Name));
            if (Players.Count == 2)
                Status = GameStatus.Playing;
        }

        public bool IsFinished()
        {
            return CurrentTurnIndex >= TurnsCount;
        }

        public void SetPlayerDecision(string userId, PlayerDecision decision)
        {
            if (Status != GameStatus.Playing)
                throw new ArgumentException(Status.ToString());
            foreach (var player in Players.Where(p => p.UserId == userId))
                player.Decision = decision;
            if (Players.All(p => p.Decision != PlayerDecision.None))
                FinishTurn();
        }

        private void FinishTurn()
        {
            if (Players[1].Decision.Beats(Players[0].Decision))
                Players[1].Score++;
            if (Players[0].Decision.Beats(Players[1].Decision))
                Players[0].Score++;

            foreach (var player in Players)
                player.Decision = PlayerDecision.None;

            CurrentTurnIndex++;
            if (CurrentTurnIndex == TurnsCount)
                Status = GameStatus.Finished;
        }
    }
}