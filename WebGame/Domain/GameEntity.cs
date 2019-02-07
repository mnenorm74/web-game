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
            //TODO pe: move to extension method for PlayerDecision
            var d0 = (int)Players[0].Decision - 1;
            var d1 = (int)Players[1].Decision - 1;
            if (d0 == (d1 + 1)%3) Players[1].Score++;
            if (d1 == (d0 + 1)%3) Players[0].Score++;
            foreach (var player in Players)
                player.Decision = PlayerDecision.None;
            if (CurrentTurnIndex == TurnsCount - 1)
                Status = GameStatus.Finished;
            else 
                CurrentTurnIndex++;
        }
    }

    public enum GameStatus
    {
        WaitingToStart,
        Playing,
        Finished,
        Canceled
    }
}