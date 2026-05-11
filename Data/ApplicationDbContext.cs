using Microsoft.EntityFrameworkCore;
using TFG.Models;

namespace TFG.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> Usuarios { get; set; }
        public DbSet<ProductoModel> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<LineaPedido> LineasPedido { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<SolicitudPresupuesto> SolicitudesPresupuesto { get; set; }
    }
}
