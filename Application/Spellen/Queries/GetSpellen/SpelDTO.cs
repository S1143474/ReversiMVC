﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Spellen.Queries.GetSpellen
{
    public class SpelDTO
    {
        public int id { get; set; }
        public string omschrijving { get; set; }
        public string token { get; set; }
        public string speler1Token { get; set; }
        public string speler2Token { get; set; }
        public List<List<int>> bord { get; set; }
        public int aandeBeurt { get; set; }
    }
}
