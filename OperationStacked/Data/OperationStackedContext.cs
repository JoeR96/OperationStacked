using System.Globalization;
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
                optionsBuilder.UseNpgsql(_connectionString, options =>
                    options.EnableRetryOnFailure());
            }
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EquipmentStack>()
                .Property(e => e.InitialIncrements)
                .HasConversion(
                    v => v != null ? string.Join(',', v) : null,
                    v => v != null
                        ? v.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(SafeParseDecimal)
                            .Where(x => x.HasValue)
                            .Select(x => x.Value)
                            .ToList() // Convert to List<decimal>, which implements ICollection<decimal>
                        : null);

                modelBuilder.Entity<WorkoutExercise>()
                    .HasMany(we => we.LinearProgressionExercises)
                    .WithOne(lpe => lpe.WorkoutExercise)
                    .HasForeignKey(lpe => lpe.WorkoutExerciseId)
                    .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<Exercise>()
                    .HasKey(e => e.Id); // Assuming 'Id' is the primary key on the Exercise table

                modelBuilder.Entity<Exercise>()
                    .HasKey(e => e.Id);

                modelBuilder.Entity<Exercise>()
                    .HasMany(e => e.ExerciseHistories)
                    .WithOne(h => h.Exercise)
                    .HasForeignKey(h => h.ExerciseId)
                    .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<Session>()
                    .HasMany(s => s.SessionExercises)
                    .WithOne() // No inverse navigation property defined in SessionExercise
                    .HasForeignKey(se => se.Id) // Assuming SessionId property is added to SessionExercise
                    .OnDelete(DeleteBehavior.Cascade); // Ensuring cascade delete

                // Configure SessionExercise and Set relationship
                modelBuilder.Entity<SessionExercise>()
                    .HasMany(se => se.Sets)
                    .WithOne() // No inverse navigation property defined in Set
                    .HasForeignKey(s => s.Id) // Assuming SessionExerciseId property is added to Set
                    .OnDelete(DeleteBehavior.Cascade); // Ensuring cascade delete

                // Configure SessionExercise and Exercise relationship
                modelBuilder.Entity<SessionExercise>()
                    .HasOne(se => se.Exercise)
                    .WithMany() // Assuming no navigation property back from Exercise to SessionExercise
                    .HasForeignKey(se => se.ExerciseId)
                    .OnDelete(DeleteBehavior.Restrict); // Assuming you don't want deleting an Exercise to cascade


        }

        public virtual DbSet<LinearProgressionExercise> LinearProgressionExercises { get; set; }
        public virtual DbSet<A2SHypertrophyExercise> A2SHypertrophyExercises { get; set; }
        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public virtual DbSet<ExerciseHistory> ExerciseHistory { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<EquipmentStack> EquipmentStacks { get; set; }
        public virtual DbSet<Workout>  Workouts { get; set; }
        public virtual DbSet<Session>  Sessions { get; set; }
        public virtual DbSet<SessionExercise>  SessionExercises { get; set; }
        public virtual DbSet<Set>  Sets { get; set; }

        public static decimal? SafeParseDecimal(string value)
        {
            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }

            return null;
        }

    }
}
