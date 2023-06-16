using MediatR;

namespace OrderEat.Application.Restaurant.Commands.CreateRestaurant
{
    public class CreateRestaurantCommand : RestaurantDto, IRequest<string>
    {

    }
}