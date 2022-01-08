using System;
using System.Collections.Generic;
using System.Text;
using Application.Spellen.Queries.GetSpellen;

namespace Application.Spellen.Queries.GetSpel
{
    public class GetSpelDTO
    {
        public int Id { get; set; }
        public string Omschrijving { get; set; }
        public string Token { get; set; }
        public string Speler1Token { get; set; }
        public string Speler2Token { get; set; }
        public List<List<int>> Bord { get; set; }
        public int AandeBeurt { get; set; }

        public string Speler1Naam { get; set; }
        public string Speler2Naam { get; set; }

    }
}
