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
            var db = TestMongoDatabase.Create();
            db.DropCollection(MongoGameRepository.CollectionName);
            repo = new MongoGameRepository(db);
        }

        private MongoGameRepository repo;

        [Test]
        public void CreateGame()
        {
            var gameEntity = repo.Create(new GameEntity(10));
            Console.WriteLine(gameEntity.Id);
            gameEntity.Id.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void FindGameById()
        {
            var gameEntity = repo.Create(new GameEntity(10));
            repo.ReadById(gameEntity.Id)
                .Should().NotBeNull();
        }

        [Test]
        public void UpdateGame()
        {
            var createdGame = repo.Create(new GameEntity(10));
            createdGame.AddPlayer(new UserEntity("userId", "Name"));
            repo.Update(createdGame);
            var retrievedGame = repo.ReadById(createdGame.Id);
            retrievedGame.Players.Should().HaveCount(1);
            retrievedGame.Players[0].UserId.Should().Be("userId");
        }
    }
}