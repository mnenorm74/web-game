using System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using WebApi.Models;
using WebGame.Domain;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class UsersController : Controller
    {
        private readonly IUserRepository userRepository;

        public UsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Получить пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <response code="200">OK</response>
        /// <response code="404">Пользователь не найден</response>
        [HttpGet("{userId}", Name = nameof(GetUserById))]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(404)]
        public virtual ActionResult<UserDto> GetUserById([FromRoute, Required] Guid userId)
        {
            var userEntity = userRepository.FindById(userId);

            if (userEntity == null)
                return NotFound();

            var user = Mapper.Map<UserDto>(userEntity);
            return Ok(user);
        }

        /// <summary>
        /// Создать пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /api/users
        ///     {
        ///        "login": "johndoe375",
        ///        "firstName": "John",
        ///        "lastName": "Doe"
        ///     }
        ///
        /// </remarks>
        /// <param name="body">Данные для создания пользователя</param>
        /// <response code="201">Пользователь создан</response>
        /// <response code="400">Некорректные входные данные</response>
        /// <response code="422">Ошибка при проверке</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [Consumes("application/json")]
        public virtual IActionResult CreateUser([FromBody] UserToCreate body)
        {
            if (body == null)
                return BadRequest();

            var userEntity = Mapper.Map<UserEntity>(body);
            userRepository.Create(userEntity);

            return CreatedAtRoute(
                nameof(GetUserById),
                new {userId = userEntity.Id},
                userEntity.Id);
        }

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <response code="204">Пользователь удален</response>
        /// <response code="404">Пользователь не найден</response>
        [HttpDelete("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public virtual IActionResult DeleteUser([FromRoute, Required] string userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Частично обновить пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="body">JSON Patch для пользователя</param>
        /// <response code="204">Пользователь обновлен</response>
        /// <response code="400">Некорректные входные данные</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="422">Ошибка при проверке</response>
        [HttpPatch("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [Consumes("application/json-patch+json")]
        public virtual IActionResult PartiallyUpdateUser([FromRoute, Required] Guid userId,
            [FromBody] JsonPatchDocument<UserToUpdate> body)
        {
            throw new NotImplementedException();
            //if (!ModelState.IsValid)
            //    return new UnprocessableEntityObjectResult(ModelState);
        }

        /// <summary>
        /// Обновить пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="body">Обновленные данные пользователя</param>
        /// <response code="204">Пользователь обновлен</response>
        /// <response code="400">Некорректные входные данные</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="422">Ошибка при проверке</response>
        [HttpPut("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [Consumes("application/json")]
        public virtual IActionResult UpdateUser([FromRoute, Required] Guid userId, [FromBody] UserToUpdate body)
        {
            throw new NotImplementedException();
        }
    }
}