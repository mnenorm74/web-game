using System;
using AutoMapper;
using NUnit.Framework;

namespace WebApi.Samples
{
    [TestFixture]
    public class AutomapperTests
    {
        class User
        {
            public string Login { get; set; }
            public string Name { get; set; }
            public DateTime RegistrationTime { get; set; }
        }

        class UserDto
        {
            public string Name { get; set; }
            public DateTime RegistrationTime { get; set; }
            public int Code { get; set; }
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<User, UserDto>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name ?? src.Login));
            });
        }

        [Test]
        public static void TestCreateNew()
        {
            var user = new User
            {
                Login = "Anonymous",
                Name = null,
                RegistrationTime = DateTime.Now.AddYears(-1)
            };
            var userDto = Mapper.Map<UserDto>(user);
            Assert.AreEqual("Anonymous", userDto.Name);
        }

        [Test]
        public static void TestFillBy()
        {
            var user = new User
            {
                Login = "Anonymous",
                Name = null,
                RegistrationTime = DateTime.Now.AddYears(-1)
            };
            var userDto = Mapper.Map(user, new UserDto
            {
                Code = 5
            });
            Assert.AreEqual("Anonymous", userDto.Name);
            Assert.AreEqual(5, userDto.Code);
        }
    }
}