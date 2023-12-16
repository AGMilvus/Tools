using EmailValidation.Models;
using Microsoft.EntityFrameworkCore;

namespace EmailValidation.EntityFrameworkCore;

public class AppDbContext : DbContext
{
	public DbSet<EmailEntity> Emails { get; set; } = null!;

	/// <inheritdoc />
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

	/// <inheritdoc />
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<EmailEntity>().HasKey(u => u.Id);
		modelBuilder.Entity<EmailEntity>(builder => builder.ToTable("Email"));
	}
	// protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	// {
	// 	optionsBuilder.UseSqlite("Data Source=emailValidation.db");
	// }
}