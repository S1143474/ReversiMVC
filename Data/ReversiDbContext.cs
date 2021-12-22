using Microsoft.EntityFrameworkCore;
using ReversiMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Data
{
    public class ReversiDbContext : DbContext
    {
        public DbSet<SpelerModel> Spelers { get; set; }

        public ReversiDbContext(DbContextOptions<ReversiDbContext> options)
            : base(options)
        {
        }

    }
}
