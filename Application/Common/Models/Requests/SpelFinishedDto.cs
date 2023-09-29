using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models.Requests
{
    public class SpelFinishedDto : SpelDto
    {
        public DateTime GameStartedAt { get; set; }
        public DateTime GameFinishedAt { get; set; }
        public Guid GameWonBy { get; set; }
        public Guid GameLostBy { get; set; }
        public int AmountOfFichesFlippedByPlayer1 { get; set; }
        public int AmountOfFichesFlippedByPlayer2 { get; set; }

    }
}
