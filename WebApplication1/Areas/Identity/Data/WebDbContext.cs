using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WebApplication1.Areas.Identity.Data;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class WebDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Member> Members { get; set; }
    public DbSet<TransactionFee> TransactionFees { get; set; }
    public DbSet<EntryLog> EntryLogs { get; set; }
    public DbSet<ConnectionRequest> ConnectionRequests { get; set; }
    public IEnumerable<object> ApplicationUsers { get; internal set; }

    public DbSet<Email> Emails { get; set; }


    public DbSet<Resource> Resources { get; set; }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<Classes> Classes { get; set; }

    public DbSet<ClassMember> ClassMembers { get; set; }

    public DbSet<ClassEmployee> ClassEmployees { get; set; }

    public DbSet<CalorieEntry> CalorieEntries { get; set; }

    public DbSet<WeightEntry> WeightEntries { get; set; }

    public WebDbContext(DbContextOptions<WebDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configuration for the one-to-many relationship
        builder.Entity<ApplicationUser>()
        .HasMany(u => u.Member)
        .WithOne(m => m.ApplicationUser)
        .HasForeignKey(m => m.UserId)
        .IsRequired();

        // Configuration for the EntryLog
        builder.Entity<EntryLog>()
        .HasIndex(e => new { e.MemberId, e.EntryDate })
        .IsUnique();

        /* // Configure foreign key for ClassEmployee
         builder.Entity<ClassEmployee>()
             .HasOne(cm => cm.Class)
             .WithMany(c => c.ClassEmployee)
             .HasForeignKey(cm => cm.ClassID)
             .OnDelete(DeleteBehavior.Restrict)
             .IsRequired();

         builder.Entity<ClassEmployee>()
             .HasOne(cm => cm.Employee)
             .WithMany()
             .HasForeignKey(cm => cm.EmployeeID)
             .OnDelete(DeleteBehavior.Restrict)
             .IsRequired();*/

        builder.Entity<ClassMember>()
       .HasOne(cm => cm.Class)
       .WithMany(c => c.ClassMember)
       .HasForeignKey(cm => cm.ClassID)
       .OnDelete(DeleteBehavior.ClientSetNull);

        builder.Entity<ClassEmployee>()
            .HasOne(ce => ce.Class)
            .WithMany(c => c.ClassEmployee)
            .HasForeignKey(ce => ce.ClassID)
            .OnDelete(DeleteBehavior.ClientSetNull)
            ;
    }
}
