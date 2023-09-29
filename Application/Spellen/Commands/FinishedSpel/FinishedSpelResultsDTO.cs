using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Spellen.Commands.FinishedSpel
{
    public class FinishedSpelResultsDTO
    {
        public Guid Token { get; set; }

        public string Description { get; set; }

        public Guid Speler1Token { get; set; }

        public Guid? Speler2Token { get; set; }

        public List<List<int>> Bord { get; set; }

        public int Turn { get; set; }

        public DateTime GameStartedAt { get; set; }
        public DateTime GameFinishedAt { get; set; }

        public Guid GameWonBy { get; set; }
        public Guid GameLostBy { get; set; }

        public int AmountOfFichesFlippedByPlayer1 { get; set; }
        public int AmountOfFichesFlippedByPlayer2 { get; set; }
        public bool IsSpelFinished { get; set; }
        public bool IsDraw { get; set; }
        public bool IsWinner { get; set; }
        public string WinnerToken { get; set; }
        public string LoserToken { get; set; }
        public int AmountOfWitFichesTurned { get; set; }
        public int AmountOfZwartFichesTurned { get; set; }
        public int AmountOfGeenFichesTurned { get; set; }

    }
}
