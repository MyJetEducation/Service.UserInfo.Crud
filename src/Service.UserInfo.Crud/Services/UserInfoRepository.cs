using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

		public async ValueTask<UserInfoEntity> GetUserInfoByNameAsync(string userName)
		{
			try
			{
				return await GetContext()
					.UserInfos
					.FirstOrDefaultAsync(entity => entity.UserName == userName);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, exception.Message);
			}

			return await ValueTask.FromResult<UserInfoEntity>(null);
		}

		public async ValueTask<UserInfoEntity> GetUserInfoByTokenAsync(string refreshToken)
		{
			try
			{
				return await GetContext()
					.UserInfos
					.FirstOrDefaultAsync(entity => entity.RefreshToken == refreshToken);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, exception.Message);
			}

			return await ValueTask.FromResult<UserInfoEntity>(null);
		}

		public async ValueTask<bool> UpdateUserTokenInfoAsync(string userName, string jwtToken, string refreshToken, DateTime? refreshTokenExpires)
		{
			UserInfoEntity userInfo = await GetUserInfoByNameAsync(userName);
			if (userInfo == null)
				return false;

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

				return true;
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, exception.Message);
			}

			return false;
		}

		public async ValueTask<bool> CreateUserInfo(string userName, string password)
		{
			try
			{
				_context = GetContext();

				await _context
					.UserInfos
					.AddAsync(new UserInfoEntity
					{
						Id = Guid.NewGuid(),
						UserName = userName,
						Password = password,
						Role = "default"
					});

				await _context.SaveChangesAsync();

				return true;
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, exception.Message);
			}

			return false;
		}

		private DatabaseContext GetContext() => DatabaseContext.Create(_dbContextOptionsBuilder);
	}
}