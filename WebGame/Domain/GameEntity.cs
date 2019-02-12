using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace WebGame.Domain
{
    public class GameEntity
    {
        public GameEntity(int turnsCount)
            : this(null, GameStatus.WaitingToStart, turnsCount, 0, new List<Player>())
        {
        }

        [BsonConstructor]
        public GameEntity(string id, GameStatus status, int turnsCount, int currentTurnIndex, List<Player> players)
        {
            Id = id;
            Status = status;
            TurnsCount = turnsCount;
            CurrentTurnIndex = currentTurnIndex;
            this.players = players;
        }

        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; private set; }

        [BsonRequired]
        private readonly List<Player> players;

        [BsonIgnore]
        public IReadOnlyList<Player> Players => players;
        [BsonRequired]
        public int TurnsCount { get; }
        [BsonRequired]
        public int CurrentTurnIndex { get; private set; }
        [BsonRequired]
        public GameStatus Status { get; private set; }

        public void AddPlayer(UserEntity user)
        {
            if (Status != GameStatus.WaitingToStart)
                throw new ArgumentException(Status.ToString());
            players.Add(new Player(user.Id, user.Name));
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
            {
                Status = GameStatus.Finished;
            }
        }
    }
}