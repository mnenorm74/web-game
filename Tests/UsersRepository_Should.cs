using System;
using System.Linq;
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
            db.DropCollection(MongoUserRepository.CollectionName);
            repo = new MongoUserRepository(db);
        }

        private MongoUserRepository repo;

        [Test]
        public void InsertUser()
        {
            var newUser = new UserEntity(Guid.Empty, "login", "last", "first", 2, null);
            var user = repo.Insert(newUser);
            user.Should().BeEquivalentTo(newUser, o => o.Excluding(u => u.Id));
            user.Id.Should().NotBe(Guid.Empty);
        }

        [Test]
        public void FindUserById()
        {
            var newUser = new UserEntity(Guid.Empty, "login", "last", "first", 2, Guid.NewGuid());
            var user = repo.Insert(newUser);
            var retrieved = repo.FindById(user.Id);
            retrieved.Should().BeEquivalentTo(user);
        }

        [Test]
        public void CreateUserByLogin()
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

        [Test]
        public void GetFirstPage()
        {
            repo.GetOrCreateByLogin("1");
            repo.GetOrCreateByLogin("2");
            repo.GetOrCreateByLogin("3");
            var page = repo.GetPage(1, 2);
            page.CurrentPage.Should().Be(1);
            page.TotalCount.Should().Be(3);
            page.PageSize.Should().Be(2);
            page.Select(u => u.Login).Should().BeEquivalentTo("1", "2");
        }

        [Test]
        public void GetSecondNotFullPage()
        {
            repo.GetOrCreateByLogin("1");
            repo.GetOrCreateByLogin("2");
            repo.GetOrCreateByLogin("3");
            var page = repo.GetPage(2, 2);
            page.CurrentPage.Should().Be(2);
            page.TotalCount.Should().Be(3);
            page.PageSize.Should().Be(2);
            page.Select(u => u.Login).Should().BeEquivalentTo("3");
        }

        [Test]
        public void Delete()
        {
            var user = repo.GetOrCreateByLogin("login");
            repo.Delete(user.Id);
            repo.FindById(user.Id).Should().BeNull();
        }


        [Test(Description = "Тест на наличие индекса по логину")]
        [Explicit("Это дополнительная задача Индекс")]
        [MaxTime(15000)]
        public void SearchByLoginFast()
        {
            for (int i = 0; i < 10000; i++)
                repo.GetOrCreateByLogin(i.ToString());
        }

        [Test(Description = "Параллельные запросы не должны падать")]
        [Explicit("Это дополнительная задача")]
        public void MassiveConcurrentCreateUser()
        {
            for (int i = 0; i < 1000; i++)
            {
                var login = "login" + i;
                Task.WaitAll(
                    Task.Run(() => repo.GetOrCreateByLogin(login)),
                    Task.Run(() => repo.GetOrCreateByLogin(login)));
            }
        }
    }
}