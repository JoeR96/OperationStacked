using Microsoft.EntityFrameworkCore;
using OperationStacked.Entities;

namespace OperationStacked.Data
{
    public class OperationStackedContext : DbContext
    {
        public OperationStackedContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           if(!optionsBuilder.IsConfigured)
            {
                var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));

                optionsBuilder.UseMySql("server=operationstacked.cjmlgmxmo9qy.eu-west-2.rds.amazonaws.com;Port=3306;Database=OperationStacked;User Id=opeartionstacked;Password=Zelfdwnq9512! ", serverVersion);
            }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<LinearProgressionExercise> LinearProgressionExercises { get; set; }
        public virtual DbSet<A2SHypertrophyExercise> A2SHypertrophyExercises { get; set; }
        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
