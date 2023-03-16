using MediatR;

namespace FastFood.Application.Restaurant.Commands.CreateRestaurant
{
    public class CreateRestaurantCommand : RestaurantDto, IRequest<string>
    {

    }
}