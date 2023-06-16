using MediatR;

namespace OrderEat.Application.Restaurant.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommand : UpdateRestaurantDto, IRequest
    {
        public int Id { get; set; }
    }
}