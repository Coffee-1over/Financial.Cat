using Financial.Cat.Domain.Models.Entities.Abstract;

namespace Financial.Cat.Domain.Models.Entities
{
	public class PurchaseEntity : IBaseEntity, IAuditDateInfo
    {
        public long Id { get; set; }

        public long UserId { get; set; }
        public long? AddressId { get; set; }

        public AddressEntity? Address { get; set; }

        public virtual IList<ItemEntity> Items { get; set; } = new List<ItemEntity>();

        public DateTime? PurchaseTime { get; set; }


		public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public UserEntity User { get; set; }
    }
}
