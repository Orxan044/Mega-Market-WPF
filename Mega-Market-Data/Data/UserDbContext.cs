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
    public DbSet<History> Histories { get; set; }
    public DbSet<ProductHistory> ProductHistories { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CreditCart>()
            .HasOne(c => c.User)
            .WithMany(u => u.CreditCarts)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ProductHistory>()
            .HasOne<History>() 
            .WithMany(h => h.Products) 
            .HasForeignKey(p => p.HistoryId) 
            .OnDelete(DeleteBehavior.NoAction); 
    }

}
