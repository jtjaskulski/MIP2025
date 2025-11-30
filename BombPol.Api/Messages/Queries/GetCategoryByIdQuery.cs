using BombPol.Api.Messages.DTOs;
using MediatR;

namespace BombPol.Api.Messages.Queries
{
    public class GetCategoryByIdQuery : IRequest<CategoryDto?>
    {
        public Guid Id { get; private set; }

        public GetCategoryByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}