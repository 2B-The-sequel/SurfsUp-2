using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Models;

namespace SurfsUp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Board> Board { get; set; } = default!;
        public DbSet<Equipment> Equipment { get; set; } = default!;
     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Board>()
            .HasMany(p => p.Equipment)
            .WithMany(p => p.Boards)
            .UsingEntity<BoardEquipment>(
                j => j
                    .HasOne(pt => pt.Equipment)
                    .WithMany(t => t.BoardEquipments)
                    .HasForeignKey(pt => pt.EquipmentId),
                j => j
                    .HasOne(pt => pt.Board)
                    .WithMany(p => p.BoardEquipments)
                    .HasForeignKey(pt => pt.BoardId),
                j =>
                {
                    j.HasKey(t => new { t.EquipmentId, t.BoardId });
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}