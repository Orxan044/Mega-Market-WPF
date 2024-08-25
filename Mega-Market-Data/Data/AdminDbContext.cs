using Mega_Market_Data.Models.Concretes;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Mega_Market_Data.Data;

public class AdminDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=ORXAN;Initial Catalog=AdminDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<DailyTotal> DailyTotals{ get; set; }
}
