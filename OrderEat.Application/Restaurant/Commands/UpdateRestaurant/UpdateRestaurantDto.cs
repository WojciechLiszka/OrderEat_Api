using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderEat.Application.Restaurant.Commands.UpdateRestaurant
{
    public class UpdateRestaurantDto
    {
        public string Description { get; set; } = default!;
        public string ContactNumber { get; set; } = default!;
        public string Email { get; set; } = default!;

        public string Country { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string ApartmentNumber { get; set; } = default!;
    }
}
