using System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.JsonPatch;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class UsersController : Controller
    {
        /// <summary>
        /// Создать пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /api/users
        ///     {
        ///        "userName": "JohnDoe",
        ///        "firstName": "John",
        ///        "lastName": "Doe",
        ///        "email": "johndoe375@gmail.com"
        ///     }
        ///
        /// </remarks>
        /// <param name="body">Данные для создания пользователя</param>
        /// <response code="201">Пользователь создан</response>
        /// <response code="400">Некорректные входные данные</response>
        /// <response code="422">Ошибка при проверке</response>
        [HttpPost]
        [Consumes("application/json")]
        public virtual IActionResult CreateUser([FromBody] UserToCreate body)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <response code="204">Пользователь удален</response>
        /// <response code="404">Пользователь не найден</response>
        [HttpDelete]
        [Route("{userId}")]
        [Consumes("application/json")]
        public virtual IActionResult DeleteUser([FromRoute, Required] Guid userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <response code="200">OK</response>
        /// <response code="404">Пользователь не найден</response>
        [HttpGet]
        [Route("{userId}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [Consumes("application/json")]
        public virtual ActionResult<UserDto> GetUserById([FromRoute, Required] Guid userId)
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
        [HttpPatch]
        [Route("{userId}")]
        [Consumes("application/json-patch+json")]
        public virtual IActionResult PartiallyUpdateUser([FromRoute, Required] Guid userId,
            [FromBody] JsonPatchDocument<UserToUpdate> body)
        {
            throw new NotImplementedException();
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
        [HttpPut]
        [Route("{userId}")]
        [Consumes("application/json")]
        public virtual IActionResult UpdateUser([FromRoute, Required] Guid userId, [FromBody] UserToUpdate body)
        {
            throw new NotImplementedException();
        }
    }
}