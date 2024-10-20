using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using tasktracker_3.DTO;
using tasktracker_3.Models;
using TaskUnit = tasktracker_3.Models.TaskUnit;

namespace tasktracker_3.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskUnit> TaskUnits { get; set; }
        public DbSet<Worker> Workers { get; set; }
        //public DbSet<TaskSelfRelation> TaskSelfReltions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Many-to-Many Project/Worker
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Workers)
                .WithMany(w => w.Projects)
                .UsingEntity<Dictionary<string, object>>(
                    "ProjectsWorkers", // Name of the join table
                    pw => pw.HasOne<Worker>().WithMany().HasForeignKey("WorkerId")
                    .OnDelete(DeleteBehavior.Restrict),
                    pw => pw.HasOne<Project>().WithMany().HasForeignKey("ProjectId")
                    .OnDelete(DeleteBehavior.Restrict)
                );

            // Many-to-Many TaskUnit/Worker
            modelBuilder.Entity<TaskUnit>()
                .HasMany(t => t.Workers)
                .WithMany(w => w.Tasks)
                .UsingEntity<Dictionary<string, object>>(
                    "WorkersTasks", // Name of the join table
                    wt => wt.HasOne<Worker>().WithMany().HasForeignKey("WorkerId")
                    .OnDelete(DeleteBehavior.Restrict),
                    wt => wt.HasOne<TaskUnit>().WithMany().HasForeignKey("TaskId")
                    .OnDelete(DeleteBehavior.Restrict)
                );

            // One-to-Many Project/TaskUnit
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Many-to-Many Project/Worker
            modelBuilder.Entity<TaskUnit>()
                .HasMany(t => t.ChildOf)
                .WithMany(t => t.ParentOf)
                .UsingEntity<Dictionary<string, object>>(
                    "TaskSelfRelation", // Name of the join table
                    pw => pw.HasOne<TaskUnit>().WithMany().HasForeignKey("ParentId")
                    .OnDelete(DeleteBehavior.Restrict),
                    tt => tt.HasOne<TaskUnit>().WithMany().HasForeignKey("ChildId")
                    .OnDelete(DeleteBehavior.Restrict)
                );

        }

    }
}
