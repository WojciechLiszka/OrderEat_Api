using MediatR;

namespace FastFood.Application.Restaurant.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommand : IRequest
    {
        public int Id { get; set; }
    }
}