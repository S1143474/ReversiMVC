using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models.Responses
{
    public class ResponseSpelDto
    {
        public string Omschrijving { get; set; }
        public Guid Token { get; set; }
        public Guid Speler1Token { get; set; }
        public Guid Speler2Token { get; set; }
        public List<List<int>> Bord { get; set; }
        public int AandeBeurt { get; set; }

        public string Speler1Naam { get; set; }
        public string Speler2Naam { get; set; }
    }
}
