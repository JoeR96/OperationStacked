using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OperationStacked.Entities;
using OperationStacked.Options;

namespace OperationStacked.Data
{
    public class OperationStackedContext : DbContext
    {
        private readonly string _connectionString;

        public OperationStackedContext(DbContextOptions options, IOptions<ConnectionStringOptions> connectionStringOptions) : base(options)
        {
            _connectionString = connectionStringOptions.Value.ConnectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
                optionsBuilder.UseMySql(_connectionString, serverVersion, options => options.EnableRetryOnFailure());
            }
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


                modelBuilder.Entity<EquipmentStack>()
                    .Property(e => e.InitialIncrements)
                    .HasConversion(
                        v => string.Join(",", v),
                        v => v.Split(new char[] { ',' }).Select(decimal.Parse).ToList());


                modelBuilder.Entity<WorkoutExercise>()
                    .HasMany(we => we.LinearProgressionExercises)
                    .WithOne(lpe => lpe.WorkoutExercise)
                    .HasForeignKey(lpe => lpe.WorkoutExerciseId)
                    .OnDelete(DeleteBehavior.Cascade);


        }

        public virtual DbSet<LinearProgressionExercise> LinearProgressionExercises { get; set; }
        public virtual DbSet<A2SHypertrophyExercise> A2SHypertrophyExercises { get; set; }
        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public virtual DbSet<ExerciseHistory> ExerciseHistory { get; set; }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<EquipmentStack> EquipmentStacks { get; set; }

    }
}
