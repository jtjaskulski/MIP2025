namespace BombPol.Data.Seeding
{
    /// <summary>
    /// Interface for database table seeders.
    /// </summary>
    public interface ISeeder
    {
        /// <summary>
        /// Seeds the database table with initial data.
        /// </summary>
        /// <returns>A task representing the asynchronous seeding operation.</returns>
        Task SeedAsync();
    }
}
