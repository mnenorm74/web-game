using System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
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
        /// <response code="200">OK</response>
        /// <response code="404">Игра не найдена</response>
        [HttpGet("{gameId}")]
        [ProducesResponseType(typeof(GameDto), 200)]
        [ProducesResponseType(404)]
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
        /// <response code="201">Игра создана</response>
        /// <response code="400">Некорректные входные данные</response>
        /// <response code="422">Ошибка при проверке</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
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
            var createdGameEntity = gameRepository.Create(gameEntity);

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
        /// <response code="204">Пользователь добавлен в качестве игрока</response>
        /// <response code="400">Некорректные входные данные</response>
        /// <response code="404">Игра или пользователь не найдены</response>
        [HttpPut("{gameId}/players/{userId}")]
        [Consumes("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
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
        /// <response code="200">OK</response>
        /// <response code="404">Игра не найдена</response>
        [HttpGet("{gameId}/status")]
        [ProducesResponseType(typeof(GameStatus), 200)]
        [ProducesResponseType(404)]
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
        /// <response code="200">OK</response>
        /// <response code="404">Игра или игрок не найдены</response>
        [HttpGet("{gameId}/players/{userId}")]
        [ProducesResponseType(typeof(PlayerDto), 200)]
        [ProducesResponseType(404)]
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
        /// <response code="200">Решение задано</response>
        /// <response code="400">Некорректные входные данные</response>
        /// <response code="404">Игра или пользователь не найдены</response>
        [HttpPost("{gameId}/players/{userId}/decision")]
        [Consumes("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
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