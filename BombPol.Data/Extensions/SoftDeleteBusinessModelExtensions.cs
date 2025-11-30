using BombPol.Data.Entities;

namespace BombPol.Data.Extensions
{
    public static class SoftDeleteBusinessModelExtensions
    {
        public static void SoftDelete(this SoftDeleteBusinessModel model)
        {
            model.DeletedAt = DateTime.UtcNow;
        }

        public static IQueryable<T> FilterOutDeleted<T>(this IQueryable<T> query) where T : SoftDeleteBusinessModel
        {
            return query.Where(e => e.DeletedAt == null);
        }
    }
}
