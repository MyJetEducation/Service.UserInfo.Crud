using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Core.Client.Constants;
using Service.Core.Client.Extensions;
using Service.UserInfo.Crud.Domain.Models;
using Service.UserInfo.Crud.Postgres;

namespace Service.UserInfo.Crud.Domain
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

		public async ValueTask<UserInfoEntity> GetUserInfoByLoginAsync(string userNameHash, string passwordHash = null) =>
			await GetByName(GetContext(), userNameHash, passwordHash);

		private async ValueTask<UserInfoEntity> GetByName(DatabaseContext context, string userNameHash, string passwordHash = null, bool onlyActive = true)
		{
			try
			{
				return await context
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

		private async ValueTask<UserInfoEntity> GetById(DatabaseContext context, Guid? userId, bool onlyActive = true)
		{
			try
			{
				return await context
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

			DatabaseContext context = GetContext();

			UserInfoEntity userInfo = await GetById(context, userId);
			if (userInfo == null)
				return false;

			userInfo.JwtToken = token;
			userInfo.RefreshToken = refreshToken;
			userInfo.RefreshTokenExpires = refreshTokenExpires;
			userInfo.IpAddress = ipAddress;

			try
			{
				await UpdateUserInfo(context, userInfo);

				return true;
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, exception.Message);
			}

			return false;
		}

		public async ValueTask<Guid?> CreateUserInfoAsync(string userNameEncoded, string userNameHash, string passwordHash, string activationHash)
		{
			DatabaseContext context = GetContext();

			UserInfoEntity userInfo = await GetByName(context, userNameHash, onlyActive: false);
			if (userInfo is { ActivationHash: null })
				return null;

			try
			{
				if (userInfo != null)
				{
					userInfo.UserName = userNameEncoded;
					userInfo.PasswordHash = passwordHash;
					userInfo.Role = UserRole.Default;
					userInfo.ActivationHash = activationHash;

					await UpdateUserInfo(context, userInfo);

					return userInfo.Id;
				}
				var userId = Guid.NewGuid();

				await context
					.UserInfos
					.AddAsync(new UserInfoEntity
					{
						Id = userId,
						UserName = userNameEncoded,
						UserNameHash = userNameHash,
						PasswordHash = passwordHash,
						Role = UserRole.Default,
						ActivationHash = activationHash
					});

				await context.SaveChangesAsync();

				return userId;
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, exception.Message);
			}

			return null;
		}

		public async ValueTask<bool> ConfirmUserInfoAsync(string activationHash)
		{
			try
			{
				DatabaseContext context = GetContext();

				UserInfoEntity userInfo = await context
					.UserInfos
					.FirstOrDefaultAsync(entity => entity.ActivationHash == activationHash);

				if (userInfo == null)
					return false;

				userInfo.ActivationHash = null;

				await UpdateUserInfo(context, userInfo);

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
			DatabaseContext context = GetContext();

			UserInfoEntity userInfo = await GetByName(context, userNameHash);
			if (userInfo == null)
				return false;

			userInfo.PasswordHash = passwordHash;

			try
			{
				await UpdateUserInfo(context, userInfo);

				return true;
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, exception.Message);
			}

			return false;
		}

		private static async Task UpdateUserInfo(DatabaseContext context, UserInfoEntity userInfo)
		{
			context
				.UserInfos
				.Update(userInfo);

			await context.SaveChangesAsync();
		}

		private DatabaseContext GetContext() => DatabaseContext.Create(_dbContextOptionsBuilder);
	}
}