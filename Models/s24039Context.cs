using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public class s24039Context : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost,5000;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=true;");
        }
    }
    //public s24039Context(DbContextOptions<s24039Context> options) : base(options)
    //{
    //}

    public DbSet<TaskType> TaskTypes { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<TeamMember> TeamMembers { get; set; }
    public DbSet<Project> Project { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfiguracja relacji między tabelami
        modelBuilder.Entity<Project>()
            .HasKey(p => p.IdTeam);

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.IdTeam);

        modelBuilder.Entity<TeamMember>()
            .HasKey(tm => tm.IdTeamMember);

        modelBuilder.Entity<TeamMember>()
            .HasMany(tm => tm.Tasks)
            .WithOne(t => t.AssignedTo)
            .HasForeignKey(t => t.IdAssignedTo);

        modelBuilder.Entity<TaskType>()
            .HasKey(tt => tt.IdTaskType);

        modelBuilder.Entity<TaskType>()
            .HasMany(tt => tt.Tasks)
            .WithOne(t => t.TaskType)
            .HasForeignKey(t => t.IdTaskType);
    }
}
