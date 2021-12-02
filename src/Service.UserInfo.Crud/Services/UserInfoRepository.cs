using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using Service.UserInfo.Crud.Postgres;
using Service.UserInfo.Crud.Postgres.Models;

namespace Service.UserInfo.Crud.Services
{
	public class UserInfoRepository : IUserInfoRepository
	{
		private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;
		private readonly ILogger<UserInfoRepository> _logger;
		private DatabaseContext _context;

		public UserInfoRepository(DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder, ILogger<UserInfoRepository> logger)
		{
			_dbContextOptionsBuilder = dbContextOptionsBuilder;
			_logger = logger;
		}

		public async Task<UserInfoEntity> GetUserInfoAsync(string userName)
		{
			try
			{
				return await GetContext().UserInfos.FirstOrDefaultAsync(entity => entity.UserName == userName);
			}
			catch (NpgsqlException exception)
			{
				_logger.LogError(exception, exception.Message);
				throw;
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				throw;
			}
		}

		public async Task UpdateUserTokenInfoAsync(string userName, string jwtToken, string refreshToken, DateTime? refreshTokenExpires)
		{
			UserInfoEntity userInfo = await GetUserInfoAsync(userName);
			if (userInfo == null)
				return;

			userInfo.JwtToken = jwtToken;
			userInfo.RefreshToken = refreshToken;
			userInfo.RefreshTokenExpires = refreshTokenExpires;

			try
			{
				_context = GetContext();

				_context
					.UserInfos
					.Update(userInfo);

				await _context.SaveChangesAsync();
			}
			catch (NpgsqlException exception)
			{
				_logger.LogError(exception, exception.Message);
				throw;
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				throw;
			}
		}

		private DatabaseContext GetContext() => DatabaseContext.Create(_dbContextOptionsBuilder);
	}
}