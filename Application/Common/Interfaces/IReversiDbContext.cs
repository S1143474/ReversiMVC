using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IReversiDbContext
    {
        DbSet<SpelerModel> Spelers { get; set; }
    }
}
