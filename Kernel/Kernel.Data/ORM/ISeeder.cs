namespace Kernel.Data.ORM
{
    public interface ISeeder
    {
        /// <summary>
        /// Gets a client identifer
        /// </summary>
        string ClientIdentifier { get; }

        byte SeedingOrder { get; }

        /// <summary>
        /// Seeds the database with initial values
        /// </summary>
        /// <param name="context"></param>
        void Seed(IDbContext context);
    }
}