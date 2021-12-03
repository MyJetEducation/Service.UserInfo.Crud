using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.UserInfo.Crud.Grpc.Contracts;
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

		private async ValueTask<UserInfoEntity> GetUserInfoById(Guid? userId)
		{
			try
			{
				return await GetContext()
					.UserInfos
					.FirstOrDefaultAsync(entity => entity.Id == userId);
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

		public async ValueTask<bool> UpdateUserTokenInfoAsync(UserNewTokenInfoRequest request)
		{
			Guid? userId = request.UserId;
			if (userId == null)
				return false;

			UserInfoEntity userInfo = await GetUserInfoById(userId);
			if (userInfo == null)
				return false;

			userInfo.JwtToken = request.JwtToken;
			userInfo.RefreshToken = request.RefreshToken;
			userInfo.RefreshTokenExpires = request.RefreshTokenExpires;
			userInfo.IpAddress = request.IpAddress;

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