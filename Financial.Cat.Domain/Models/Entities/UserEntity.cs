using Financial.Cat.Domain.Models.Entities.Abstract;

namespace Financial.Cat.Domain.Models.Entities
{
	public class UserEntity : IBaseEntity, IAuditDateInfo
    {
		public long Id { get; set; }

		public string? Email { get; set; }

		public string PasswordHash { get; set; }

		public string? TwoFactorKey { get; set; }

		public bool TwoFactorEnabled { get; set; }

		public bool EmailVerified { get; set; }

		public bool IsActive { get; set; }

		public ICollection<PurchaseEntity> Purchases { get; set; } = [];
		public ICollection<ItemNomenclatureEntity> ItemNomenclatures { get; set; } = [];
		public ICollection<AuthOperationEntity> AuthOperations { get; set; } = [];
		public ICollection<OtpEntity> Otps { get; set; } = [];
		public ICollection<SettingLimitEntity> SettingLimits { get; set; } = [];

		public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
    }
}