using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IReversiDbContext
    {
        DbSet<Speler> Spelers { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
