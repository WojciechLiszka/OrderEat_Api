using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderEat.Application.Dish.Command.DeleteDish
{
    public class DeleteDishCommand :IRequest
    {
        public int id { get; set; }
    }
}
