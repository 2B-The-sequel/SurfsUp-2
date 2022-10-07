using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Models;

namespace SurfsUp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public DbSet<Board> Board { get; set; } = default!;
        public DbSet<Equipment> Equipment { get; set; } = default!;
        public DbSet<BoardEquipment> BoardEquipment { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Board>()
            .HasMany(p => p.Equipment)
            .WithMany(p => p.Boards)
            .UsingEntity<BoardEquipment>(
                j => j
                    .HasOne(pt => pt.Equipment)
                    .WithMany(t => t.BoardEquipments)
                    .HasForeignKey(pt => pt.EquipmentId)
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne(pt => pt.Board)
                    .WithMany(p => p.BoardEquipments)
                    .HasForeignKey(pt => pt.BoardId)
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey(t => new { t.EquipmentId, t.BoardId });
                });

            modelBuilder.Entity<Rental>()
            .HasOne(p => p.Board)
            .WithMany(p => p.rentals)
            .HasForeignKey(p => p.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rental>()
                .HasOne(p => p.User)
                .WithMany(p => p.rentals)
                .HasForeignKey(p => p.UsersId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}