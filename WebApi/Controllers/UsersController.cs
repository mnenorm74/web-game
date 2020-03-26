using System;
using System.Linq;
using AutoMapper;
using Game.Domain;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        // Чтобы ASP.NET положил что-то в userRepository требуется конфигурация
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpGet("{userId}", Name = nameof(GetUserById))]
        [Produces("application/json", "application/xml")]
        public ActionResult<UserDto> GetUserById([FromRoute] Guid userId)
        {
            var user = userRepository.FindById(userId);
            if (user is null)
            {
                return NotFound();
            }

            var mappedUser = mapper.Map<UserDto>(user);

            return Ok(mappedUser);
        }

        [HttpPost]
        [Produces("application/json", "application/xml")]
        public IActionResult CreateUser([FromBody] CreatingUserDto user)
        {
            if (user is null)
            {
                return BadRequest();
            }

            if (user.Login is null || !user.Login.All(char.IsLetterOrDigit))
            {
                ModelState.AddModelError(nameof(CreatingUserDto.Login), "Invalid login");
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var mappedUser = mapper.Map<UserEntity>(user);
            var createdUser = userRepository.Insert(mappedUser);

            return CreatedAtRoute(
                nameof(GetUserById),
                new {userId = createdUser.Id},
                createdUser.Id);
        }

        [HttpPut("{userId}")]
        [Produces("application/json", "application/xml")]
        public IActionResult UpdateUser([FromRoute] Guid userId, [FromBody] UpdatingUserDto user)
        {
            if (user is null || userId == Guid.Empty)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var createdUser = new UserEntity(userId);
            var mappedUser = mapper.Map<UserEntity>(createdUser);
            userRepository.UpdateOrInsert(mappedUser, out var isInserted);

            if (isInserted)
            {
                return CreatedAtRoute(nameof(GetUserById), new {userId = createdUser.Id}, createdUser.Id);
            }

            return NoContent();
        }

        [HttpPatch("{userId}")]
        [Produces("application/json", "application/xml")]
        public IActionResult PartiallyUpdateUser([FromRoute] Guid userId,
            [FromBody] JsonPatchDocument<UpdatingUserDto> patchDocument)
        {
            if (patchDocument is null)
            {
                return BadRequest();
            }

            var user = userRepository.FindById(userId);
            if (user is null)
            {
                return NotFound();
            }

            var mappedUpdatingUser = mapper.Map<UpdatingUserDto>(user);
            patchDocument.ApplyTo(mappedUpdatingUser, ModelState);
            
            if (!ModelState.IsValid || !TryValidateModel(mappedUpdatingUser))
            {
                return UnprocessableEntity(ModelState);
            }

            var mappedUser = mapper.Map<UserEntity>(user);
            userRepository.Update(mappedUser);

            return NoContent();
        }
    }
}