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
            var gameEntity = repo.Insert(new GameEntity(10));
            Console.WriteLine(gameEntity.Id);
            gameEntity.Id.Should().NotBe(Guid.Empty);
        }

        [Test]
        public void FindGameById()
        {
            var gameEntity = repo.Insert(new GameEntity(10));
            repo.FindById(gameEntity.Id)
                .Should().NotBe(Guid.Empty);
        }

        [Test]
        public void UpdateGame()
        {
            var createdGame = repo.Insert(new GameEntity(10));
            var login = "someUserName";
            createdGame.AddPlayer(new UserEntity { Login = login });
            repo.Update(createdGame);
            var retrievedGame = repo.FindById(createdGame.Id);
            retrievedGame.Players.Should().HaveCount(1);
            retrievedGame.Players[0].Name.Should().Be(login);
        }
    }
}