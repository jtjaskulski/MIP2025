using BombPol.Api.Messages.DTOs;
using MediatR;

namespace BombPol.Api.Messages.Queries
{
    public class GetAllCategoriesQuery : IRequest<List<CategoryDto>>
    {
    }
}
