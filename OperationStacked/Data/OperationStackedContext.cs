using Microsoft.EntityFrameworkCore;
using OperationStacked.Entities;

namespace OperationStacked.Data
{
    public class OperationStackedContext : DbContext
{
    public OperationStackedContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<LinearProgressionExercise> LinearProgressionExercises { get; set; }
    public virtual DbSet<A2SHypertrophyExercise> A2SHypertrophyExercises { get; set; }
    public virtual DbSet<Exercise> Exercises { get; set; }
    public virtual DbSet<User> Users { get; set; }
}

}
