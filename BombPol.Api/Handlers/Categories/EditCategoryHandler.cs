using BombPol.Api.Messages.Commands.Categories;
using BombPol.Data;
using BombPol.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BombPol.Api.Handlers.Categories
{
    public class EditCategoryHandler(BombPolContext context, ILogger<EditCategoryHandler> logger)
        : IRequestHandler<EditCategoryCommand, Unit>
    {
        public async Task<Unit> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Edytowanie kategorii bombeczek: {Id}", request.Id);

            if (request.Id == Guid.Empty || !await context.Category.FilterOutDeleted().AnyAsync(cat => cat.Id == request.Id))
            {
                throw new KeyNotFoundException($"Kategoria bombeczek o ID {request.Id} nie istnieje");
            }

            var category = await context.Category.FindAsync(request.Id, cancellationToken);

            if (category == null)
            {
                throw new KeyNotFoundException($"Kategoria bombeczek o ID {request.Id} nie istnieje");
            }

            category.Name =  request.Name;
            category.Description = request.Description ?? string.Empty;

            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Edytowano kategorię ID: {Id}", category.Id);

            return Unit.Value;
        }
    }
}