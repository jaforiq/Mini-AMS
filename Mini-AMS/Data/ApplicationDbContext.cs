using Mini_AMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Mini_AMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<VoucherHeader> VoucherHeaders { get; set; }
        public DbSet<VoucherLine> VoucherLines { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Accountant", NormalizedName = "ACCOUNTANT" },
                new IdentityRole { Id = "3", Name = "Viewer", NormalizedName = "VIEWER" }
            );

            builder.Entity<ChartOfAccount>()
                .HasOne<ChartOfAccount>()
                .WithMany()
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<VoucherHeader>()
                .HasMany(v => v.Lines)
                .WithOne(l => l.VoucherHeader)
                .HasForeignKey(l => l.VoucherHeaderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 