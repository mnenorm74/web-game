using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Models;
using WebGame.Domain;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class UsersController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly LinkGenerator linkGenerator;

        public UsersController(IUserRepository userRepository, LinkGenerator linkGenerator)
        {
            this.userRepository = userRepository;
            this.linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Получить пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        [HttpGet("{userId}", Name = nameof(GetUserById))]
        [HttpHead("{userId}")]
        [SwaggerResponse(200, "OK", typeof(UserDto))]
        [SwaggerResponse(404, "Пользователь не найден")]
        public ActionResult<UserDto> GetUserById([FromRoute, Required] Guid userId)
        {
            var userFromRepo = userRepository.FindById(userId);

            if (userFromRepo == null)
                return NotFound();

            var user = Mapper.Map<UserDto>(userFromRepo);
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
        /// <param name="user">Данные для создания пользователя</param>
        [HttpPost]
        [Consumes("application/json")]
        [SwaggerResponse(201, "Пользователь создан")]
        [SwaggerResponse(400, "Некорректные входные данные")]
        [SwaggerResponse(422, "Ошибка при проверке")]
        public IActionResult CreateUser([FromBody] UserToCreateDto user)
        {
            if (user == null)
                return BadRequest();

            if (!user.Login.All(c => char.IsLetter(c) || char.IsDigit(c)))
            {
                ModelState.AddModelError(nameof(UserToCreateDto),
                    "Login should contain only letters or digits.");
            }

            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            var userEntity = Mapper.Map<UserEntity>(user);
            var createdUserEntity = userRepository.Insert(userEntity);

            return CreatedAtRoute(
                nameof(GetUserById),
                new {userId = createdUserEntity.Id},
                createdUserEntity.Id);
        }

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        [HttpDelete("{userId}")]
        [SwaggerResponse(204, "Пользователь удален")]
        [SwaggerResponse(404, "Пользователь не найден")]
        public IActionResult DeleteUser([FromRoute, Required] Guid userId)
        {
            var userFromRepo = userRepository.FindById(userId);
            if (userFromRepo == null)
                return NotFound();

            userRepository.Delete(userId);
            return NoContent();
        }

        /// <summary>
        /// Обновить пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="user">Обновленные данные пользователя</param>
        [HttpPut("{userId}")]
        [Consumes("application/json")]
        [SwaggerResponse(201, "Пользователь создан")]
        [SwaggerResponse(204, "Пользователь обновлен")]
        [SwaggerResponse(400, "Некорректные входные данные")]
        [SwaggerResponse(422, "Ошибка при проверке")]
        public IActionResult UpdateUser([FromRoute, Required] Guid userId, [FromBody] UserToUpdateDto user)
        {
            if (user == null)
                return BadRequest();

            ValidateUserToUpdate(user);
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            var userFromRepo = userRepository.FindById(userId);
            if (userFromRepo == null)
            {
                var userEntity = Mapper.Map<UserEntity>(new UserEntity(userId));
                userRepository.UpdateOrInsert(userEntity);

                return CreatedAtRoute(
                    nameof(GetUserById),
                    new {userId = userEntity.Id},
                    userEntity.Id);
            }

            Mapper.Map(user, userFromRepo);
            userRepository.Update(userFromRepo);
            return NoContent();
        }

        /// <summary>
        /// Частично обновить пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="patchDoc">JSON Patch для пользователя</param>
        [HttpPatch("{userId}")]
        [Consumes("application/json-patch+json")]
        [SwaggerResponse(204, "Пользователь обновлен")]
        [SwaggerResponse(400, "Некорректные входные данные")]
        [SwaggerResponse(404, "Пользователь не найден")]
        [SwaggerResponse(422, "Ошибка при проверке")]
        public IActionResult PartiallyUpdateUser([FromRoute, Required] Guid userId,
            [FromBody] JsonPatchDocument<UserToUpdateDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var userFromRepo = userRepository.FindById(userId);
            if (userFromRepo == null)
                NotFound();
            
            var user = Mapper.Map<UserToUpdateDto>(userFromRepo);

            patchDoc.ApplyTo(user, ModelState);
            ValidateUserToUpdate(user);
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            Mapper.Map(user, userFromRepo);
            userRepository.Update(userFromRepo);

            return NoContent();
        }

        /// <summary>
        /// Получить пользователей
        /// </summary>
        /// <param name="pageNumber">Номер страницы, по умолчанию 1</param>
        /// <param name="pageSize">Размер страницы, по умолчанию 20</param>
        /// <response code="200">OK</response>
        [HttpGet(Name = nameof(GetUsers))]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
        public IActionResult GetUsers(int pageNumber = 1, int pageSize = 10)
        {
            pageNumber = Math.Max(pageNumber, 1);
            pageSize = Math.Min(Math.Max(pageSize, 1), 20);
            var pageList = userRepository.GetPage(pageNumber, pageSize);

            var users = Mapper.Map<IEnumerable<UserDto>>(pageList);

            var paginationHeader = new
            {
                previousPageLink = pageList.HasPrevious
                    ? CreateGetUsersUri(pageList.CurrentPage - 1, pageList.PageSize)
                    : null,
                nextPageLink = pageList.HasNext
                    ? CreateGetUsersUri(pageList.CurrentPage + 1, pageList.PageSize)
                    : null,
                totalCount = pageList.TotalCount,
                pageSize = pageList.PageSize,
                currentPage = pageList.CurrentPage,
                totalPages = pageList.TotalPages
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationHeader));

            return Ok(users);
        }

        /// <summary>
        /// Опции по запросам о пользователях
        /// </summary>
        [HttpOptions]
        [SwaggerResponse(200, "OK")]
        public IActionResult GetUsersOptions()
        {
            Response.Headers.Add("Allow", "POST,GET,OPTIONS");
            return Ok();
        }

        private string CreateGetUsersUri(int pageNumber, int pageSize)
        {
            return linkGenerator.GetUriByRouteValues(HttpContext, nameof(GetUsers),
                new
                {
                    pageNumber,
                    pageSize
                });
        }

        private void ValidateUserToUpdate(UserToUpdateDto user)
        {
            if (!user.Login.All(c => char.IsLetter(c) || char.IsDigit(c)))
            {
                ModelState.AddModelError(nameof(UserToUpdateDto),
                    "Login should contain only letters or digits.");
            }
        }
    }
}