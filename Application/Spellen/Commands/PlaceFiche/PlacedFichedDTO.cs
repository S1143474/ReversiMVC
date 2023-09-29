using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interfaces;
using Application.Common.Models;

namespace Application.Spellen.Commands.PlaceFiche
{
    public class PlacedFichedDTO : BaseDTO
    {
        public bool IsSpelFinished { get; set; }

        public bool IsDraw { get; set; }

        public string WinnerToken { get; set; }

        public string LoserToken { get; set; }

        public Guid PlacedBySpelerToken { get; set; }

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

