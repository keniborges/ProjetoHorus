using Microsoft.EntityFrameworkCore;
using ProjetoAula.Entidades;

namespace ProjetoAula.Contexts
{
	public class HorusContext : DbContext
	{

		public HorusContext(DbContextOptions<HorusContext> options) : base(options) { }

		public DbSet<Cliente> Cliente { get; set; }

		public DbSet<Endereco> Endereco { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
