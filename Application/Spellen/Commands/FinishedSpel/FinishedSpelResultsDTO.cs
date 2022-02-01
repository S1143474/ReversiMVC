using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Spellen.Commands.FinishedSpel
{
    public class FinishedSpelResultsDTO
    {
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
