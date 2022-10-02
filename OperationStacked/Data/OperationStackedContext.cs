using Bogus;
using Microsoft.EntityFrameworkCore;
using OperationStacked.Entities;
using OperationStacked.Models;
using OperationStacked.Requests;

namespace OperationStacked.Data
{
    public class OperationStackedContext : DbContext
    {
        public OperationStackedContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=ms-sql-server, 1433;Database=OperationStacked;User Id=SA;Password=Zelfdwnq9512!;Encrypt=False;TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
            



        }
        public virtual DbSet<LinearProgressionExercise> LinearProgressionExercises { get; set; }

        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
