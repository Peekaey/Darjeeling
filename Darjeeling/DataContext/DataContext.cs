using Darjeeling.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Darjeeling.Repositories;

public class DataContext : DbContext
{
    public DbSet<FCGuildServer> FCGuildServers { get; set; }
    public DbSet<FCGuildRole> FCGuildRoles { get; set; }
    public DbSet<FCGuildMember> FCMembers { get; set; }
    public DbSet<LodestoneNameHistory> LodestoneNameHistories { get; set; }
    public DbSet<DiscordNameHistory> DiscordNameHistories { get; set; }
    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}