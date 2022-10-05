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

                optionsBuilder.UseMySql("server=ls-6b84692ae3e7c001eb2dd8e32baa790294ae17bb.cpofuaa7ukod.eu-west-2.rds.amazonaws.com;Port=3306;Database=OperationStacked;User Id=admin;Password=Zelfdwnq9512! ", serverVersion);
                //optionsBuilder.UseSqlServer("Server=ms-sql-server, 1433;Database=OperationStacked;User Id=SA;Password=Zelfdwnq9512!;Encrypt=False;TrustServerCertificate=True;");
            }
            base.OnConfiguring(optionsBuilder);
            



        }
        public virtual DbSet<LinearProgressionExercise> LinearProgressionExercises { get; set; }

        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
