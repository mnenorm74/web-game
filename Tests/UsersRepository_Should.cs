using System;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Driver;
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
        public void DontCreateUserWithSameLogin()
        {
            var user = repo.GetOrCreateByLogin("login");
            var user2 = repo.GetOrCreateByLogin("login");
            user2.Id.Should().Be(user.Id);
        }

        [Test]
        public void FindUserById()
        {
            var user = repo.GetOrCreateByLogin("login");
            var retrieved = repo.FindById(user.Id);
            Assert.NotNull(retrieved);
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
            Assert.NotNull(retrieved);
            retrieved.CurrentGameId.Should().Be(gameId);
        }

        [Test(Description = "“ест на наличие индекса по логину")]
        [Explicit("Ёто дополнительна€ задача »ндекс")]
        [MaxTime(10000)]
        public void SearchByLoginFast()
        {
            for (int i = 0; i < 10000; i++)
                repo.GetOrCreateByLogin(i.ToString());
        }

        [Test(Description = "ѕараллельные запросы не должны падать")]
        [Explicit("Ёто дополнительна€ задача")]
        public void MassiveConcurrentCreateUser()
        {
            try
            {
                for (int i = 0; i < 1000; i++)
                {
                    var login = "login" + i;
                    Task.WaitAll(
                        Task.Run(() => repo.GetOrCreateByLogin(login)),
                        Task.Run(() => repo.GetOrCreateByLogin(login)));
                }

            }
            catch (AggregateException e)
            {
                var cmd = (MongoCommandException)(e.InnerExceptions[0]);
                Console.WriteLine(cmd.Code);
                Console.WriteLine(cmd.CodeName);
                Console.WriteLine(cmd.ErrorMessage);
                Console.WriteLine(cmd.Command);
                Console.WriteLine(cmd.Result);
                Console.WriteLine(string.Join(",", cmd.ErrorLabels));
                Assert.Fail("Multiple concurrent GetOrCreateByLogin Failed!");
            }
        }
    }
}