using MediatR;

namespace BombPol.Api.Messages.Commands.Categories
{
    public class DeleteCategoryCommand : IRequest<Unit>
    {
        public Guid Id { get; private set; }

        public DeleteCategoryCommand(Guid id)
        {
                Id = id;
        }
    }
}