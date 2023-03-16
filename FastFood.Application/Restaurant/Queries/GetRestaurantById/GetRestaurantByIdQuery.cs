using MediatR;

namespace FastFood.Application.Restaurant.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQuery : IRequest<GetRestaurantDto>
    {
        public int Id { get; set; }
    }
}