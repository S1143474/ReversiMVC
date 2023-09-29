using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;

namespace Infrastructure.Persistence
{
    public class ReversiDbContext : DbContext, IReversiDbContext
    {
        private readonly IMediator _mediator;

        public ReversiDbContext(DbContextOptions<ReversiDbContext> options) : base(options)
        {

        }

        public ReversiDbContext(DbContextOptions<ReversiDbContext> options, IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<Speler> Spelers { get; set; }

        /*protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditableEntitySaveSchangesInterceptor);
        }*/

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
