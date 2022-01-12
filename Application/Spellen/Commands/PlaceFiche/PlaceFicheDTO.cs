using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Spellen.Commands.PlaceFiche
{
    public class PlaceFicheDTO
    {
        public bool IsPlaceExecuted { get; set; }

        public List<FicheCoordDTO> FichesToTurnAround { get; set; }
    }
    public class FicheCoordDTO
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}

