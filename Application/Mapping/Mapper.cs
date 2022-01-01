using System;
using System.Collections.Generic;
using System.Text;
using Application.Spellen.Queries.GetSpel;
using Application.Spellen.Queries.GetSpellen;

namespace Application.Mapping
{
    public static class Mapper
    {
        public static GetSpelDTO MapToGetSpelDto(this SpelDTO spel)
        {
            return new GetSpelDTO()
            {
                Id = spel.id,
                Omschrijving = spel.omschrijving,
                Token = spel.token,
                Speler1Token = spel.speler1Token,
                Speler2Token = spel.speler2Token,
                Bord = spel.bord,
                AandeBeurt = spel.aandeBeurt
            };
        }
    }
}
