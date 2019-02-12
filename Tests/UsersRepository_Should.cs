using System;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using NUnit.Framework;
using WebGame.Domain;

namespace Tests
{
    [TestFixture]
    public class UsersRepository_Should
    {
        [SetUp]
        public void SetUp()
        {
            var db = TestMongoDatabase.Create();
            db.DropCollection(MongoUserRepositoty.CollectionName);
            repo = new MongoUserRepositoty(db);
        }

        private MongoUserRepositoty repo;

        [Test]
        public void CreateUser()
        {
            var user = repo.GetOrCreateByLogin("login");
            Console.WriteLine(user.Id);
            user.Login.Should().Be("login");
            user.Id.Should().NotBe(Guid.Empty);
        }

        [Test]
        public void FindUserById()
        {
            var user = repo.GetOrCreateByLogin("login");
            var retrieved = repo.FindById(user.Id);
            retrieved.Login.Should().Be("login");
        }

        [Test]
        public void UpdateUser()
        {
            var gameId = Guid.NewGuid();
            var user = repo.GetOrCreateByLogin("login");
            user.CurrentGameId = gameId;
            repo.Update(user);
            var retrieved = repo.FindById(user.Id);
            retrieved.CurrentGameId.Should().Be(gameId);
        }
    }
}