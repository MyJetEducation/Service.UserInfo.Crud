using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Core.Domain.Extensions;
using Service.Core.Domain.Models.Constants;
using Service.UserInfo.Crud.Domain.Models;
using Service.UserInfo.Crud.Postgres;

namespace Service.UserInfo.Crud.Domain
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

		public async ValueTask<UserInfoEntity> GetUserInfoByLoginAsync(string userNameHash, string passwordHash = null) =>
			await GetUserInfoByNameAsync(userNameHash, passwordHash);

		private async ValueTask<UserInfoEntity> GetUserInfoByNameAsync(string userNameHash, string passwordHash = null, bool onlyActive = true)
		{
			try
			{
				return await GetContext()
					.UserInfos
					.WhereIf(onlyActive, entity => entity.ActivationHash == null)
					.WhereIf(passwordHash != null, entity => entity.PasswordHash == passwordHash)
					.FirstOrDefaultAsync(entity => entity.UserNameHash == userNameHash);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, exception.Message);
			}

			return await ValueTask.FromResult<UserInfoEntity>(null);
		}

		private async ValueTask<UserInfoEntity> GetUserInfoById(Guid? userId, bool onlyActive = true)
		{
			try
			{
				return await GetContext()
					.UserInfos
					.WhereIf(onlyActive, entity => entity.ActivationHash == null)
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
					.Where(entity => entity.ActivationHash == null)
					.FirstOrDefaultAsync(entity => entity.RefreshToken == refreshToken);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, exception.Message);
			}

			return await ValueTask.FromResult<UserInfoEntity>(null);
		}

		public async ValueTask<bool> UpdateUserTokenInfoAsync(Guid? userId, string token, string refreshToken, DateTime? refreshTokenExpires, string ipAddress)
		{
			if (userId == null)
				return false;

			UserInfoEntity userInfo = await GetUserInfoById(userId);
			if (userInfo == null)
				return false;

			userInfo.JwtToken = token;
			userInfo.RefreshToken = refreshToken;
			userInfo.RefreshTokenExpires = refreshTokenExpires;
			userInfo.IpAddress = ipAddress;

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

		public async ValueTask<bool> CreateUserInfoAsync(string userNameEncoded, string userNameHash, string passwordHash, string activationHash)
		{
			UserInfoEntity userInfo = await GetUserInfoByNameAsync(userNameHash, onlyActive: false);
			if (userInfo is { ActivationHash: null })
				return false;

			try
			{
				_context = GetContext();

				if (userInfo != null)
				{
					userInfo.UserName = userNameEncoded;
					userInfo.PasswordHash = passwordHash;
					userInfo.Role = UserRole.Default;
					userInfo.ActivationHash = activationHash;
				}
				else
					await _context
						.UserInfos
						.AddAsync(new UserInfoEntity
						{
							Id = Guid.NewGuid(),
							UserName = userNameEncoded,
							UserNameHash = userNameHash,
							PasswordHash = passwordHash,
							Role = UserRole.Default,
							ActivationHash = activationHash
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

		public async ValueTask<bool> ConfirmUserInfoAsync(string activationHash)
		{
			try
			{
				_context = GetContext();

				UserInfoEntity userInfo = await _context
					.UserInfos
					.FirstOrDefaultAsync(entity => entity.ActivationHash == activationHash);

				if (userInfo == null)
					return false;

				userInfo.ActivationHash = null;

				await _context.SaveChangesAsync();

				return true;
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, exception.Message);
			}

			return false;
		}

		public async ValueTask<bool> ChangeUserInfoPasswordAsync(string userNameHash, string passwordHash)
		{
			UserInfoEntity userInfo = await GetUserInfoByLoginAsync(userNameHash);
			if (userInfo == null)
				return false;

			try
			{
				userInfo.PasswordHash = passwordHash;

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