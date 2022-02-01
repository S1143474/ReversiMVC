using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Spellen.Commands.PlaceFiche
{
    public class PlacedFichedDTO 
    {
        public bool IsSpelFinished { get; set; }

        public bool IsDraw { get; set; }

        public string WinnerToken { get; set; }

        public string LoserToken { get; set; }

        public string PlacedBySpelerToken { get; set; }

        public int AanDeBeurt { get; set; }

        public bool IsPlaceExecuted { get; set; }

        public string NotExecutedMessage { get; set; }

        public List<FicheCoordDTO> FichesToTurnAround { get; set; }
    }
    public class FicheCoordDTO
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}

