using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models.Requests;
using Application.Spellen.Queries.GetSpel;
using Application.Spellen.Queries.GetSpellen;

namespace Application.Mapping
{
    public static class Mapper
    {
        public static GetSpelDTO MapToGetSpelDto(this SpelDto spel)
        {
            return new GetSpelDTO()
            {
                Omschrijving = spel.Description,
                Token = spel.Token.ToString(),
                Speler1Token = spel.Speler1Token.ToString(),
                Speler2Token = spel.Speler2Token.ToString(),
                Bord = spel.Bord,
                AandeBeurt = spel.Turn
            };
        }
    }
}
