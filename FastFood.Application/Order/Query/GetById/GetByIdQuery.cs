using MediatR;

namespace FastFood.Application.Order.Query.GetById
{
    public class GetByIdQuery : IRequest<Domain.Entities.Order>
    {
        public int Id { get; set; }
    }
}