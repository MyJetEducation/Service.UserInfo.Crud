using Microsoft.EntityFrameworkCore;
using MyJetWallet.Sdk.Postgres;
using MyJetWallet.Sdk.Service;
using Service.UserInfo.Crud.Domain.Models;

namespace Service.UserInfo.Crud.Postgres
{
	public class DatabaseContext : MyDbContext
	{
		public const string Schema = "education";
		private const string UserInfoTableName = "userinfo";

		public DatabaseContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<UserInfoEntity> UserInfos { get; set; }

		public static DatabaseContext Create(DbContextOptionsBuilder<DatabaseContext> options)
		{
			MyTelemetry.StartActivity($"Database context {Schema}")?.AddTag("db-schema", Schema);

			return new DatabaseContext(options.Options);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasDefaultSchema(Schema);

			SetUserInfoEntityEntry(modelBuilder);

			base.OnModelCreating(modelBuilder);
		}

		private static void SetUserInfoEntityEntry(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserInfoEntity>().ToTable(UserInfoTableName);
			modelBuilder.Entity<UserInfoEntity>().HasKey(e => e.Id);

			modelBuilder.Entity<UserInfoEntity>().Property(e => e.UserName).IsRequired();
			modelBuilder.Entity<UserInfoEntity>().Property(e => e.UserNameHash).IsRequired();
			modelBuilder.Entity<UserInfoEntity>().Property(e => e.PasswordHash).IsRequired();
			modelBuilder.Entity<UserInfoEntity>().Property(e => e.Role).IsRequired();
			modelBuilder.Entity<UserInfoEntity>().Property(e => e.JwtToken).IsRequired(false).HasMaxLength(800);
			modelBuilder.Entity<UserInfoEntity>().Property(e => e.RefreshToken).IsRequired(false).HasMaxLength(100);
			modelBuilder.Entity<UserInfoEntity>().Property(e => e.RefreshTokenExpires).IsRequired(false);
			modelBuilder.Entity<UserInfoEntity>().Property(e => e.IpAddress).IsRequired(false);
			modelBuilder.Entity<UserInfoEntity>().Property(e => e.ActivationHash);

			modelBuilder.Entity<UserInfoEntity>().HasIndex(e => e.Id).IsUnique();
			modelBuilder.Entity<UserInfoEntity>().HasIndex(e => e.UserNameHash).IsUnique();
			modelBuilder.Entity<UserInfoEntity>().HasIndex(e => e.ActivationHash);
		}
	}
}