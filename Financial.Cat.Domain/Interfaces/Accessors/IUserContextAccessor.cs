using Financial.Cat.Domain.Models.Business.AuthModels.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Application.Accessors
{
	public interface IUserContextAccessor
	{
		/// <summary>
		/// User context
		/// </summary>
		UserContextModel UserContextModel { get; set; }
	}
}
