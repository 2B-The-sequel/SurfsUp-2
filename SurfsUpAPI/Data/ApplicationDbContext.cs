using Microsoft.EntityFrameworkCore;
using SurfsUpAPI.Models;

namespace SurfsUpAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Board> Board { get; set; } = default!;
        public DbSet<Equipment> Equipment { get; set; } = default!;
        public DbSet<BoardEquipment> BoardEquipment { get; set; } = default!;
        public DbSet<Rental> Rental { get; set; } = default!;
    }
}