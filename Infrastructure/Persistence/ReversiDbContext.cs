using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Persistence
{
    public class ReversiDbContext : DbContext, IReversiDbContext
    {
        public DbSet<SpelerModel> Spelers { get; set; }

        public ReversiDbContext(DbContextOptions<ReversiDbContext> options)
            : base(options)
        {
        }

    }
}
