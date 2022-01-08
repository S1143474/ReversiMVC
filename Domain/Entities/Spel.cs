using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Spel
    {
        public int ID { get; set; }
        public string Omschrijving { get; set; }
        public string Token { get; set; }
        public string Speler1Token { get; set; }
        public string Speler2Token { get; set; }
        public List<List<int>> Bord { get; set; }
        public int AandeBeurt { get; set; }
    }
}
