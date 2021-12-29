using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SpelerModel
    {
        [Key]
        public string Guid { get; set; }

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
        public SpelerModel(string guid, string naam)
        {
            Guid = guid;
            Naam = naam;
            AantalGewonnen = 0;
            AantalVerloren = 0;
            AantalGelijk = 0;
            AantalFicheFliped = 0;
        }
    }
}
