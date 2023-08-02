using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Common;

namespace Domain.Entities
{
    public class Spel : BaseAuditableEntity
    {
        public int ID { get; set; }
        [Column("Description")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "De omschrijving moet minaal 3 characters bevatten en mag maximaal 100 lang zijn.")]
        public string Omschrijving { get; set; }
        public string Token { get; set; }
        public string Speler1Token { get; set; }
        public string Speler2Token { get; set; }
        public List<List<int>> Bord { get; set; }
        public int AandeBeurt { get; set; }
    }
}
