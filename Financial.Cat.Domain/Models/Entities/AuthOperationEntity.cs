using Financial.Cat.Domain.Enums;
using Financial.Cat.Domain.Models.Entities.Abstract;

namespace Financial.Cat.Domain.Models.Entities
{
    public class AuthOperationEntity : IBaseEntity, IAuditDateInfo
	{
		public long Id { get; set; }

		public long UserId { get; set; }

		public UserEntity User { get; set; }

		public AuthType AuthType { get; set; }
		public OperationStatusType OperationStatus { get; set; }
		public DateTime Created { get; set; }

		public DateTime Updated { get; set; }
	}
}
