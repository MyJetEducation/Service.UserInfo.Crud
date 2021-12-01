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

		public UserInfoRepository(DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder, ILogger<UserInfoRepository> logger)
		{
			_dbContextOptionsBuilder = dbContextOptionsBuilder;
			_logger = logger;
		}

		public async Task<UserInfoEntity> GetUserInfoAsync(string userName, string password)
		{
			try
			{
				return await GetContext()
					.UserInfos
					.FirstOrDefaultAsync(entity => entity.UserName == userName && entity.Password == password);
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

		public async Task<UserInfoEntity> GetUserInfoAsync(string refreshToken)
		{
			try
			{
				return await GetContext()
					.UserInfos
					.FirstOrDefaultAsync(entity => entity.RefreshToken != null && entity.RefreshToken == refreshToken);
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