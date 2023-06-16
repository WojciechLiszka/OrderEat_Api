using AutoMapper;
using Domain.Domain.Exceptions;
using OrderEat.Domain.Interfaces;
using MediatR;

namespace OrderEat.Application.SpecialDiet.Queries.GetSpecialDietById
{
    public class GetSpecialDietByIdQueryHandler : IRequestHandler<GetSpecialDietByIdQuery, GetDietDto>
    {
        private readonly ISpecialDietRepository _repository;
        private readonly IMapper _mapper;

        public GetSpecialDietByIdQueryHandler(ISpecialDietRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetDietDto> Handle(GetSpecialDietByIdQuery request, CancellationToken cancellationToken)
        {
            var diet = await _repository.GetById(request.Id);
            if (diet == null)
            {
                throw new NotFoundException("Diet not found");
            }
            var dto = _mapper.Map<GetDietDto>(diet);
            return dto;
        }
    }
}