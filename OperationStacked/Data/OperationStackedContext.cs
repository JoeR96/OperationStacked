using Amazon.Runtime;
using Amazon.SimpleSystemsManagement.Model;
using Amazon.SimpleSystemsManagement;
using Microsoft.EntityFrameworkCore;
using OperationStacked.Entities;
using Microsoft.Extensions.Options;

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

        public virtual DbSet<LinearProgressionExercise> LinearProgressionExercises { get; set; }
        public virtual DbSet<A2SHypertrophyExercise> A2SHypertrophyExercises { get; set; }
        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
