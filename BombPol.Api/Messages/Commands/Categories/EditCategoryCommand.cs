using MediatR;

namespace BombPol.Api.Messages.Commands.Categories
{
    public class EditCategoryCommand : IRequest<Unit>
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }

        public EditCategoryCommand(Guid id, string name, string description = null)
        {
            Id = id;
            Name = name;
            Description = description; 
        }
    }
}