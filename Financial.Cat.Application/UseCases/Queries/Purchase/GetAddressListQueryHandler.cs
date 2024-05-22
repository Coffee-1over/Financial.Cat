using AutoMapper;
using Financial.Cat.Domain.Interfaces.Repositories;
using Financial.Cat.Domain.Models.Dto.Out.Purchase;
using Financial.Cat.Domain.Models.Queries.Purchase;
using MediatR;

namespace Financial.Cat.Application.UseCases.Queries.Purchase
{
	internal class GetAddressListQueryHandler : IRequestHandler<GetAddressListQuery, IList<AddressOutDto>>
	{
		private readonly IAddressRepository _addressRepository;
		private readonly IMapper _mapper;

		public GetAddressListQueryHandler(IAddressRepository addressRepository, IMapper mapper)
		{
			_addressRepository = addressRepository;
			_mapper = mapper;
		}

		public async Task<IList<AddressOutDto>> Handle(GetAddressListQuery request, CancellationToken cancellationToken)
		{
			var addresses = await _addressRepository.GetAsync(null, cancellationToken);

			return _mapper.Map<IList<AddressOutDto>>(addresses);
		}
	}
}
