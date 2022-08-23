using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models.Requests
{
    public class SpelCreateDto
    {
        public string Description { get; set; }
        public Guid Speler1Token { get; set; }
    }
}
