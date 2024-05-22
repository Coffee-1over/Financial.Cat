using Financial.Cat.Domain.Models.Entities.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Financial.Cat.Domain.Models.Entities
{
	public class CategoryEntity : IBaseEntity, IAuditDateInfo
	{
		public long Id { get; set; }
		public string Name { get; set; }

		public long? ParentCategoryId { get; set; }

		[NotMapped]
		public List<CategoryEntity>? ChildCategories { get; set; }

		public ICollection<ItemNomenclatureEntity> ItemNomenclatures { get; set; } = new List<ItemNomenclatureEntity>();
		public DateTime Created { get; set; }

		public DateTime Updated { get; set; }
	}
}
