using Financial.Cat.Domain.Models.Dto.Out.Purchase;
using Financial.Cat.Domain.Models.Entities;
using MediatR;
using NetTopologySuite.Geometries;

namespace Financial.Cat.Domain.Models.Commands.Purchase
{

    public class CreatePurchaseCommand : IRequest<PurchaseOutDto>
    {
        public DateTime? PurchaseTime { get; set; }
        public ICollection<CreateItem> Items { get; set; }
        public CreateAddress? Address { get; set; }
    }

    public class CreateAddress
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string? Street1 { get; set; }
        public string? Zip { get; set; }
        public string? City { get; set; }
		public double? X { get; set; }
		public double? Y { get; set; }
		public string Country { get; set; } = null!;
    }

    public class CreateItem
    {
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public CreateItemNomenclature ItemNomenclature { get; set; }
    }

    public class CreateItemNomenclature
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public CreateCategory Category { get; set; }
        public bool IsHidden { get; set; }
    }

    public class CreateCategory
    {
        public long? Id { get; set; }
        public string Name { get; set; }

        public long? ParentCategoryId { get; set; }
    }
}
