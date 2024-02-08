using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Búsquedas.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Producto>? Productos { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
