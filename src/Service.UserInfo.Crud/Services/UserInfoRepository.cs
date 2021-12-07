using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.UserInfo.Crud.Extensions;
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

		public async ValueTask<UserInfoEntity> GetUserInfoByLoginAsync(string userNameHash, string passwordHash = null)
		{
			try
			{
				return await GetContext()
					.UserInfos
					.Where(entity => entity.ActivationHash == null)
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

		public async ValueTask<string> CreateUserInfoAsync(string userName, string userNameHash, string passwordHash)
		{
			try
			{
				_context = GetContext();

				string activationHash = GenerateHash();

				await _context
					.UserInfos
					.AddAsync(new UserInfoEntity
					{
						Id = Guid.NewGuid(),
						UserName = userName,
						UserNameHash = userNameHash,
						PasswordHash = passwordHash,
						Role = "default",
						ActivationHash = activationHash
					});

				await _context.SaveChangesAsync();

				return activationHash;
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, exception.Message);
			}

			return null;
		}

		public async ValueTask<bool> ConfirmUserInfoAsync(string hash)
		{
			try
			{
				_context = GetContext();

				UserInfoEntity userInfo = await _context
					.UserInfos
					.FirstOrDefaultAsync(entity => entity.ActivationHash == hash);

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

		private DatabaseContext GetContext() => DatabaseContext.Create(_dbContextOptionsBuilder);

		private static string GenerateHash() => Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
	}
}