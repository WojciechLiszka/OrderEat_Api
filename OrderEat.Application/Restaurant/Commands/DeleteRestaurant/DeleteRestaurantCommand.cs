using MediatR;

namespace OrderEat.Application.Restaurant.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommand : IRequest
    {
        public int Id { get; set; }
    }
}