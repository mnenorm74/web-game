using System;
using FluentAssertions;
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
            repo = new MongoUserRepositoty(TestMongoDatabase.Create());
        }

        private MongoUserRepositoty repo;

        [Test]
        public void CreateUser()
        {
            var newId = Guid.NewGuid().ToString();
            var user = repo.ReadOrCreateUser(newId);
            user.Id.Should().Be(newId);
        }

        [Test]
        public void FindUserById()
        {
            var newId = Guid.NewGuid().ToString();
            var user = repo.ReadOrCreateUser(newId);
            var retrieved = repo.ReadById(newId);
            retrieved.Should().BeEquivalentTo(user);
        }

        [Test]
        public void UpdateUser()
        {
            var newId = Guid.NewGuid().ToString();
            var user = repo.ReadOrCreateUser(newId);
            user.CurrentGameId = "42";
            repo.Update(user);
            var retrieved = repo.ReadById(newId);
            retrieved.CurrentGameId.Should().Be("42");
        }
    }
}