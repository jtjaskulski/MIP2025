using MediatR;

namespace BombPol.Api.Messages.Commands.Categories
{
    public class AddCategoryCommand : IRequest<Guid>
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }

        public AddCategoryCommand(string name, string description = null)
        {
            Name = name;
            Description = description ?? string.Empty;
        }
    }
}