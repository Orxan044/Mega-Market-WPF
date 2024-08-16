using Mega_Market_Data.Models.Concretes;
using Microsoft.EntityFrameworkCore;

namespace Mega_Market_Data.Data;

public class UserDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=ORXAN;Initial Catalog=UserDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    }
    public DbSet<User> Users { get; set; }
    public DbSet<CreditCart> CreditCarts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>()
            .HasMany(u => u.CreditCarts)
            .WithOne(cc => cc.User)
            .HasForeignKey(cc => cc.UserId);
    }
}
