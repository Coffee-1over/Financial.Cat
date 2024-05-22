using Financial.Cat.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Cat.Infrustructure.Extensions.DbExtenstions
{
	internal static class OneToOneExtensions
	{
		public static ModelBuilder ConfigureOneToOneEntities(this ModelBuilder modelBuilder)
		{
			/*modelBuilder.Entity<ItemNomenclatureEntity>()
				.HasOne(c => c.Category)
				.WithOne(i => i.Contact)
				.HasForeignKey<ContactDetailDb>(b => b.ContactId)
				.IsRequired();*/

			return modelBuilder;
		}
	}
}
