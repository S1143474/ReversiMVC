using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Spelers.Queries.GetSpeler
{
    public class SpelerDTO
    {
        public string Name { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int AmountFichesFlipped { get; set; }

    }
}
