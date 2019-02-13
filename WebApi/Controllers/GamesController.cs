using System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Models;
using WebGame.Domain;

namespace WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class GamesController : Controller
    {
        //todo метод создания игры

        /// <summary>
        /// Добавить игрока в игру
        /// </summary>
        /// <remarks>
        /// Имя игрока в каждой игре может быть уникальным, поэтому его надо передать.
        /// В случае нескольких запросов для одного пользователя, должно быть установлено имя из последнего запроса.
        /// </remarks>
        /// <param name="gameId">Идентификатор игры</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="body">Имя игрока</param>
        /// <response code="204">Пользователь добавлен в качестве игрока</response>
        /// <response code="400">Некорректные входные данные</response>
        /// <response code="404">Игра или пользователь не найдены</response>
        /// <response code="422">Ошибка при проверке</response>
        [HttpPut("{gameId}/players/{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [Consumes("application/json")]
        public virtual IActionResult AddPlayerToGame([FromRoute, Required] Guid gameId,
            [FromRoute, Required] Guid userId, [FromBody] PlayerToUpdate body)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить игру
        /// </summary>
        /// <param name="gameId">Идентификатор игры</param>
        /// <response code="200">OK</response>
        /// <response code="404">Игра не найдена</response>
        [HttpGet("{gameId}")]
        [ProducesResponseType(typeof(GameDto), 200)]
        [ProducesResponseType(404)]
        public virtual ActionResult<GameDto> GetGameById([FromRoute, Required] Guid gameId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить статус игры
        /// </summary>
        /// <param name="gameId">Идентификатор игры</param>
        /// <response code="200">OK</response>
        /// <response code="404">Игра не найдена</response>
        [HttpGet("{gameId}/status")]
        [ProducesResponseType(typeof(GameStatus), 200)]
        [ProducesResponseType(404)]
        public virtual ActionResult<GameStatus> GetGameStatusById([FromRoute, Required] Guid gameId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить игрока в игре
        /// </summary>
        /// <param name="gameId">Идентификатор игры</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <response code="200">OK</response>
        /// <response code="404">Игра или пользователь не найдены</response>
        [HttpGet("{gameId}/players/{userId}")]
        [ProducesResponseType(typeof(PlayerDto), 200)]
        [ProducesResponseType(404)]
        public virtual ActionResult<PlayerDto> GetPlayerOfGame([FromRoute, Required] Guid gameId,
            [FromRoute, Required] Guid userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Задать решение игрока
        /// </summary>
        /// <param name="gameId">Идентификатор игры</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="body">Решение игрока</param>
        /// <response code="201">Решение задано</response>
        /// <response code="400">Некорректные входные данные</response>
        /// <response code="404">Игра или пользователь не найдены</response>
        /// <response code="422">Неверное состояние игры</response>
        [HttpPost("{gameId}/players/{userId}/decision")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [Consumes("application/json")]
        public virtual IActionResult SetPlayerDecision([FromRoute, Required] Guid gameId,
            [FromRoute, Required] Guid userId, [FromBody] PlayerDecision body)
        {
            throw new NotImplementedException();
        }
    }
}