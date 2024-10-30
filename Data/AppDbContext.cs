using Microsoft.EntityFrameworkCore;
using WebApplication1.Estudantes;

namespace WebApplication1.Data
{
	public class AppDbContext : DbContext
	{
		public DbSet<Estudante> Estudantes { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=localhost;Database=webapi;User Id=sa;Password=12345678;TrustServerCertificate=True;");
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
			optionsBuilder.EnableSensitiveDataLogging();
		}

	}
}
