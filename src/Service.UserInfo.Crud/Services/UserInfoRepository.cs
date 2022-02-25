using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Core.Client.Constants;
using Service.Core.Client.Extensions;
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

		public async ValueTask<UserInfoEntity> GetByLoginAsync(string userNameHash, string passwordHash = null) =>
			await GetByName(GetContext(), userNameHash, passwordHash);

		public async ValueTask<UserInfoEntity> GetByIdAsync(Guid? userId) =>
			await GetById(GetContext(), userId);

		public async ValueTask<Guid?> CreateAsync(string userNameEncoded, string userNameHash, string passwordHash)
		{
			DatabaseContext context = GetContext();

			UserInfoEntity userInfo = await GetByName(context, userNameHash, onlyActive: false);
			if (userInfo is { Active: true })
			{
				_logger.LogError("Error while create user! User already registered and activated (userNameHash: {userNameHash}).", userNameHash);

				return null;
			}

			try
			{
				if (userInfo != null)
				{
					userInfo.UserName = userNameEncoded;
					userInfo.PasswordHash = passwordHash;
					userInfo.Role = UserRole.Default;
					userInfo.Active = null;

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
						Role = UserRole.Default
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

		public async ValueTask<string> ActivateAsync(Guid? userId)
		{
			try
			{
				DatabaseContext context = GetContext();

				UserInfoEntity userInfo = await GetById(context, userId, false);

				if (userInfo == null)
				{
					_logger.LogError("Error while confirm user! Can't find user with id: {userId}.", userId);

					return await ValueTask.FromResult<string>(null);
				}

				userInfo.Active = true;

				await UpdateUserInfo(context, userInfo);

				return userInfo.UserName;
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, exception.Message);
			}

			return await ValueTask.FromResult<string>(null);
		}

		public async ValueTask<bool> ChangePasswordAsync(string userNameHash, string passwordHash)
		{
			DatabaseContext context = GetContext();

			UserInfoEntity userInfo = await GetByName(context, userNameHash);
			if (userInfo == null)
			{
				_logger.LogError("Error while change user info! Can't find user with userNameHash: {userNameHash}.", userNameHash);

				return false;
			}

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

		public async ValueTask<bool> ChangeUserNameAsync(Guid? userId, string userNameEncoded, string userNameHash)
		{
			try
			{
				DatabaseContext context = GetContext();

				UserInfoEntity userInfo = await GetById(context, userId);

				if (userInfo == null)
				{
					_logger.LogError("Error while change user name! Can't find user with id: {userId}.", userId);

					return false;
				}

				userInfo.UserName = userNameEncoded;
				userInfo.UserNameHash = userNameHash;

				await UpdateUserInfo(context, userInfo);

				return true;
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, exception.Message);
			}

			return false;
		}

		private async ValueTask<UserInfoEntity> GetByName(DatabaseContext context, string userNameHash, string passwordHash = null, bool onlyActive = true)
		{
			try
			{
				return await context
					.UserInfos
					.WhereIf(onlyActive, entity => entity.Active == true)
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
					.WhereIf(onlyActive, entity => entity.Active == true)
					.FirstOrDefaultAsync(entity => entity.Id == userId);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, exception.Message);
			}

			return await ValueTask.FromResult<UserInfoEntity>(null);
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