using MediatR;

namespace FastFood.Application.Restaurant.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommand : UpdateRestaurantDto, IRequest
    {
        public int Id { get; set; }
    }
}