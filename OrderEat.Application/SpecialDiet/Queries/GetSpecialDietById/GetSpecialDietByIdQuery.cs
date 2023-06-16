using MediatR;

namespace OrderEat.Application.SpecialDiet.Queries.GetSpecialDietById
{
    public class GetSpecialDietByIdQuery : IRequest<GetDietDto>
    {
        public int Id { get; set; }
    }
}