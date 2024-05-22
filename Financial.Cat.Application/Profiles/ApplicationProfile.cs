using AutoMapper;
using Financial.Cat.Domain.Models.Dto.Out.Purchase;
using Financial.Cat.Domain.Models.Dto.Out.UserSetting;
using Financial.Cat.Domain.Models.Entities;

namespace Financial.Cat.Application.Profiles
{
	public class ApplicationProfile : Profile
	{
		public ApplicationProfile()
		{
			CreateMap<SettingLimitEntity, UserSettingOutDto>();
			CreateMap<ItemEntity, ItemOutDto>();
			CreateMap<CategoryEntity, CategoryOutDto>();
			CreateMap<ItemNomenclatureEntity, ItemNomenclatureOutDto>();
			CreateMap<PurchaseEntity, PurchaseOutDto>();
			CreateMap<AddressEntity, AddressOutDto>()
				.ForMember(dest => dest.X, opt => opt.MapFrom(src => src.Point != null ? src.Point.X : new double?()))
				.ForMember(dest => dest.Y, opt => opt.MapFrom(src => src.Point != null ? src.Point.Y : new double?()));




		}
	}
}
