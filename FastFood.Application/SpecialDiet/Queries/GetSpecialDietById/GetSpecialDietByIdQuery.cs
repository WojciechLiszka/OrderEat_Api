using MediatR;

namespace FastFood.Application.SpecialDiet.Queries.GetSpecialDietById
{
    public class GetSpecialDietByIdQuery : IRequest<GetDietDto>
    {
        public int Id { get; set; }
    }
}