using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ReversiDbInitializer
    {
        private readonly ReversiDbContext _context;

        public ReversiDbInitializer(ReversiDbContext context) 
        {
            _context = context;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();

            if (!_context.Spelers.Any())
            {
                var seeder = new ReversiDBSeeder(_context);
                seeder.Seed();
            }

        }
    }
}
