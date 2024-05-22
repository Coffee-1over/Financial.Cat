using Financial.Cat.Domain.Models.Business.AuthModels.Context;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Application.Accessors
{
	public class UserContextAccessor : IUserContextAccessor
	{
		private static readonly AsyncLocal<UserContextHolder> _userContextCurrent = new();

		private readonly IHttpContextAccessor _httpContextAccessor;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="httpContextAccessor">Http accessor fabric</param>
		public UserContextAccessor(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
		/// User context
		/// </summary>
		public UserContextModel UserContextModel
		{
			get
			{
				var userContext = GetUserContextFromHttpContext() ?? _userContextCurrent.Value?.contextModel;

				if (userContext == null)
					throw new InvalidOperationException("Can not get user context. Execution context must have user context");

				return userContext;
			}
			set
			{
				var holder = _userContextCurrent.Value;
				if (holder != null)
				{
					// Clear current context trapped in the AsyncLocals, as its done.
					holder.contextModel = null!;
				}

				if (value != null)
				{
					// Use an object indirection to hold the context in the AsyncLocal,
					// so it can be cleared in all ExecutionContexts when its cleared.
					_userContextCurrent.Value = new UserContextHolder { contextModel = value };
				}
			}
		}

		private UserContextModel? GetUserContextFromHttpContext()
		{
			var user = _httpContextAccessor.HttpContext?.User;

			if (user == null)
				return null;

			var idClaim = user.FindFirst(nameof(ClaimTypes.NameIdentifier));
			var id = idClaim == null ? 0 : long.Parse(idClaim.Value);

			var email = user.FindFirst(nameof(ClaimTypes.Email))?.Value;

			return new()
			{
				Id = id,
				Email = email,
			};
		}

		private class UserContextHolder
		{
			public UserContextModel contextModel;
		}
	}
}
