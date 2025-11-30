using BombPol.Api.Messages.Commands.Categories;
using BombPol.Data;
using MediatR;
using BombPol.Data.Extensions;

namespace BombPol.Api.Handlers.Categories
{
    public class DeleteCategoryHandler(BombPolContext context, ILogger<DeleteCategoryHandler> logger)
        : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Usuwanie kategorii bombeczek: {Id}", request.Id);
            
            if (request.Id == Guid.Empty || !context.Category.FilterOutDeleted().Any(cat => cat.Id == request.Id))
            {
                throw new KeyNotFoundException($"Kategoria bombeczek o ID {request.Id} nie istnieje");
            }
            
            var category = await context.Category.FindAsync(request.Id, cancellationToken);
           
            if (category == null)
            {
                throw new KeyNotFoundException($"Kategoria bombeczek o ID {request.Id} nie istnieje");
            }
            
            context.Remove(category);

            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Usunięto kategorię ID: {Id}", category.Id);

            return Unit.Value;
        }
    }
}