using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("Speler")]
    public class Speler
    {
        [Key]
        public Guid Guid { get; set; }

        public string Naam { get; set; }
        public int AantalGewonnen { get; set; }
        public int AantalVerloren { get; set; }
        public int AantalGelijk { get; set; }

        public int AantalFicheFliped { get; set; }

        /// <summary>
        /// Initial Constructor
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="naam"></param>
        public Speler(Guid guid, string naam)
        {
            Guid = guid;
            Naam = naam;
            AantalGewonnen = 0;
            AantalVerloren = 0;
            AantalGelijk = 0;
            AantalFicheFliped = 0;
        }

        public Speler(string guid, string naam)
        {
            Guid = Guid.Parse(guid);
            Naam = naam;
            AantalGewonnen = 0;
            AantalVerloren = 0;
            AantalGelijk = 0;
            AantalFicheFliped = 0;
        }
    }
}
