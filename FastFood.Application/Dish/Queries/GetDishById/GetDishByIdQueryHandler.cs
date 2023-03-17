using AutoMapper;
using Domain.Domain.Exceptions;
using FastFood.Domain.Interfaces;
using MediatR;

namespace FastFood.Application.Dish.Queries.GetDishById
{
    public class GetDishByIdQueryHandler : IRequestHandler<GetDishByIdQuery, GetDishDto>
    {
        private readonly IDishRepository _dishRepository;
        private readonly IMapper _mapper;

        public GetDishByIdQueryHandler(IDishRepository dishRepository, IMapper mapper)
        {
            _dishRepository = dishRepository;
            _mapper = mapper;
        }

        public async Task<GetDishDto> Handle(GetDishByIdQuery request, CancellationToken cancellationToken)
        {
            var dish = await _dishRepository.GetById(request.DishId);

            if (dish == null)
            {
                throw new NotFoundException("Dish not found");
            }

            var dto = _mapper.Map<GetDishDto>(dish);

            return dto;
        }
    }
}