using EmailValidation.Models;
using Microsoft.EntityFrameworkCore;

namespace EmailValidation.EntityFrameworkCore;

public class AppDbContext : DbContext
{
	public DbSet<EmailEntity> Emails { get; set; } = null!;
	
	/// <inheritdoc />
	public AppDbContext(DbContextOptions options) : base(options)
	{
	}

	/// <inheritdoc />
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<EmailEntity>(builder => builder.ToTable("Email"));
	}
}