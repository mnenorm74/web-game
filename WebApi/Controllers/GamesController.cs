using System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Models;
using WebGame.Domain;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class GamesController : Controller
    {
        private readonly IGameRepository gameRepository;
        private readonly IUserRepository userRepository;

        public GamesController(IGameRepository gameRepository, IUserRepository userRepository)
        {
            this.gameRepository = gameRepository;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Получить игру
        /// </summary>
        /// <param name="gameId">Идентификатор игры</param>
        [HttpGet("{gameId}")]
        [SwaggerResponse(200, "OK", typeof(GameDto))]
        [SwaggerResponse(404, "Игра не найдена")]
        public ActionResult<GameDto> GetGameById([FromRoute, Required] Guid gameId)
        {
            var gameFromRepo = gameRepository.FindById(gameId);

            if (gameFromRepo == null)
                return NotFound();

            var game = Mapper.Map<GameDto>(gameFromRepo);
            return Ok(game);
        }

        /// <summary>
        /// Создать игру
        /// </summary>
        /// <param name="game">Данные для создания игры</param>
        [HttpPost]
        [Consumes("application/json")]
        [SwaggerResponse(201, "Игра создана")]
        [SwaggerResponse(400, "Некорректные входные данные")]
        [SwaggerResponse(422, "Ошибка при проверке")]
        public IActionResult CreateGame([FromBody] GameToCreateDto game)
        {
            if (game == null)
                return BadRequest();

            if (game.TurnsCount < 1)
            {
                ModelState.AddModelError(nameof(GameToCreateDto),
                    "TurnsCount should be a positive number.");
            }
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            var gameEntity = new GameEntity(game.TurnsCount);
            var createdGameEntity = gameRepository.Insert(gameEntity);

            return CreatedAtRoute(
                nameof(GetGameById),
                new {gameId = createdGameEntity.Id},
                createdGameEntity.Id);
        }

        /// <summary>
        /// Добавить игрока в игру
        /// </summary>
        /// <remarks>
        /// Имя игрока в каждой игре может быть уникальным, поэтому его надо передать.
        /// В случае нескольких запросов для одного пользователя, должно быть установлено имя из последнего запроса.
        /// </remarks>
        /// <param name="gameId">Идентификатор игры</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="player">Имя игрока</param>
        [HttpPut("{gameId}/players/{userId}")]
        [Consumes("application/json")]
        [SwaggerResponse(204, "Пользователь добавлен в качестве игрока")]
        [SwaggerResponse(400, "Некорректные входные данные")]
        [SwaggerResponse(404, "Игра или пользователь не найдены")]
        public IActionResult AddPlayerToGame(
            [FromRoute, Required] Guid gameId,
            [FromRoute, Required] Guid userId)
        {
            var gameFromRepo = gameRepository.FindById(gameId);
            if (gameFromRepo == null)
                return NotFound();

            var userFromRepo = userRepository.FindById(userId);
            if (userFromRepo == null)
                return NotFound();

            try
            {
                gameFromRepo.AddPlayer(userFromRepo);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// Получить статус игры
        /// </summary>
        /// <param name="gameId">Идентификатор игры</param>
        [HttpGet("{gameId}/status")]
        [SwaggerResponse(200, "OK", typeof(GameStatus))]
        [SwaggerResponse(404, "Игра не найдена")]
        public ActionResult<GameStatus> GetGameStatusById([FromRoute, Required] Guid gameId)
        {
            var gameFromRepo = gameRepository.FindById(gameId);

            if (gameFromRepo == null)
                return NotFound();

            return Ok(gameFromRepo.Status);
        }

        /// <summary>
        /// Получить игрока в игре
        /// </summary>
        /// <param name="gameId">Идентификатор игры</param>
        /// <param name="userId">Идентификатор пользователя</param>
        [HttpGet("{gameId}/players/{userId}")]
        [SwaggerResponse(200, "OK", typeof(PlayerDto))]
        [SwaggerResponse(404, "Игра или игрок не найдены")]
        public ActionResult<PlayerDto> GetPlayerOfGame(
            [FromRoute, Required] Guid gameId,
            [FromRoute, Required] Guid userId)
        {
            var gameFromRepo = gameRepository.FindById(gameId);
            if (gameFromRepo == null)
                return NotFound();

            var playerFromRepo = gameFromRepo.Players.FirstOrDefault(p => p.UserId == userId);
            if (playerFromRepo == null)
                return NotFound();

            var player = Mapper.Map<PlayerDto>(playerFromRepo);
            return Ok(player);
        }

        /// <summary>
        /// Задать решение игрока
        /// </summary>
        /// <param name="gameId">Идентификатор игры</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="decision">Решение игрока</param>
        /// <response code="200"></response>
        /// <response code="400"></response>
        /// <response code="404">Игра или пользователь не найдены</response>
        [HttpPost("{gameId}/players/{userId}/decision")]
        [Consumes("application/json")]
        [SwaggerResponse(204, "Решение задано")]
        [SwaggerResponse(400, "Некорректные входные данные")]
        [SwaggerResponse(404)]
        public IActionResult SetPlayerDecision(
            [FromRoute, Required] Guid gameId,
            [FromRoute, Required] Guid userId,
            [FromBody] PlayerDecision decision)
        {
            var gameFromRepo = gameRepository.FindById(gameId);
            if (gameFromRepo == null)
                return NotFound();

            var playerFromRepo = gameFromRepo.Players.FirstOrDefault(p => p.UserId == userId);
            if (playerFromRepo == null)
                return NotFound();

            try
            {
                gameFromRepo.SetPlayerDecision(userId, decision);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }
    }
}