using Financial.Cat.Domain.Models.Entities.Abstract;

namespace Financial.Cat.Domain.Models.Entities
{
	public class ItemNomenclatureEntity : IBaseEntity, IAuditDateInfo
	{
		public long Id { get; set; }
		public string Name { get; set; }

		public long UserId { get; set; }
		public UserEntity User { get; set; }
		public CategoryEntity Category { get; set; }
		public long CategoryId { get; set; }

		public bool IsHidden { get; set; }

		public DateTime Created { get; set; }

		public DateTime Updated { get; set; }

		public virtual IList<ItemEntity> Items { get; set; } = new List<ItemEntity>();
	}
}
