using Darjeeling.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Darjeeling.Repositories;

public class DataContext : DbContext
{
    public DbSet<FCGuildServer> FCGuildServers { get; set; }
    public DbSet<FCGuildRole> FCGuildRoles { get; set; }
    public DbSet<FCMember> FCMembers { get; set; }
    public DbSet<NameHistory> NameHistories { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}