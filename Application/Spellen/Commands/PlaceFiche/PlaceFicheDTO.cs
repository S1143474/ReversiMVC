using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.Spellen.Commands.PlaceFiche
{
    public class PlaceFicheDTO
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool HasPassed { get; set; }
    }
}
