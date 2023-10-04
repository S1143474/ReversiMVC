using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Seeder
{
    public class ReversiDBSeeder
    {

        private readonly ReversiDbContext _dbContext;

        public ReversiDBSeeder(ReversiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            foreach (var speler in Spelers)
            {
                _dbContext.Spelers.Add(speler);
            }

            _dbContext.SaveChanges();
        }

        private static List<Speler> Spelers = new List<Speler>
        {
            new Speler(Guid.NewGuid(), "TestAdmin"),
            new Speler(Guid.NewGuid(), "TestModerator"),
            new Speler(Guid.NewGuid(), "TestSpeler")
        };
    }
}
