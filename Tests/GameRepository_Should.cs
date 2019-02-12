using System;
using FluentAssertions;
using NUnit.Framework;
using WebGame.Domain;

namespace Tests
{
    [TestFixture]
    public class GameRepository_Should
    {
        [SetUp]
        public void SetUp()
        {
            repo = new MongoGameRepository(TestMongoDatabase.Create());
        }

        private MongoGameRepository repo;

        [Test]
        public void CreateGame()
        {
            var gameEntity = repo.Create(new GameEntity());
            Console.WriteLine(gameEntity.Id);
            gameEntity.Id.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void FindGameById()
        {
            var gameEntity = repo.Create(new GameEntity());
            repo.ReadById(gameEntity.Id)
                .Should().NotBeNull();
        }

        [Test]
        public void UpdateGame()
        {
            var createdGame = repo.Create(new GameEntity());
            var player = new Player("userId", "someName");
            createdGame.Players.Add(player);
            repo.Update(createdGame);
            var retrievedGame = repo.ReadById(createdGame.Id);
            retrievedGame.Players.Should().HaveCount(1);
            retrievedGame.Players[0].UserId.Should().Be("userId");
        }
    }
}