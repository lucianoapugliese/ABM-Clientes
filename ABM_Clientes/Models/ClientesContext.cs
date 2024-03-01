using Microsoft.EntityFrameworkCore;

namespace ABM_Clientes.Models
{
    public class ClientesContext : DbContext
    {
        public ClientesContext(DbContextOptions<ClientesContext> opciones) : base(opciones)
        {
            
        }
        public DbSet<Clientes> Clientes { get; set; }
    }
}
