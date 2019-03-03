using System;
using System.Collections.Generic;
using WebGame.Domain;

namespace WebApi.Models
{
    public class GameDto
    {
        public Guid Id { get; set; }
        public List<PlayerDto> Players { get; set; }
        public int TurnsCount { get; set; }
        public int CurrentTurnIndex { get; set; }
        public GameStatus Status { get; set; }
    }
}