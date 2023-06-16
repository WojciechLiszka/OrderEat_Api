using MediatR;

namespace OrderEat.Application.Restaurant.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQuery : IRequest<GetRestaurantDto>
    {
        public int Id { get; set; }
    }
}