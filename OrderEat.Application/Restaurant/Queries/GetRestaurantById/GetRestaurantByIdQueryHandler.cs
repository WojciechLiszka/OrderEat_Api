using AutoMapper;
using Domain.Domain.Exceptions;
using OrderEat.Domain.Interfaces;
using MediatR;

namespace OrderEat.Application.Restaurant.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQueryHandler : IRequestHandler<GetRestaurantByIdQuery, GetRestaurantDto>
    {
        private readonly IRestaurantRepository _repository;
        private readonly IMapper _mapper;

        public GetRestaurantByIdQueryHandler(IRestaurantRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetRestaurantDto> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
        {
            var restaurant = await _repository.GetById(request.Id);
            if (restaurant == null)
            {
                throw new NotFoundException($"Restaurant with Id:{request.Id} doesn't exist");
            }
            var dto = _mapper.Map<GetRestaurantDto>(restaurant);
            return dto;
        }
    }
}