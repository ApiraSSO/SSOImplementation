using System.Data.Entity;
using System.Linq;
using Kernel.Data;
using Kernel.Data.Connection;
using Kernel.Data.ORM;
using Kernel.Reflection;
using Provider.EntityFramework.Initialisation;

namespace Provider.EntityFramework
{
    public class DBContext : DbContext, IDbContext
    {
        public IDbCustomConfiguration CustomConfiguration { get; }

        static DBContext()
		{
			Database.SetInitializer(new DbInitialiser());
		}
		/// <summary>
		/// Constructor with connection string. Used by derived classes which have own connection string factory
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="identityProvider"></param>
		public DBContext(IConnectionStringProvider connectionString, IDbCustomConfiguration customConfiguration)
			: base(connectionString.GetConnectionString().ConnectionString)
        {
            this.CustomConfiguration = customConfiguration;
        }

		/// <summary>
		///     This method is called when the model for a derived context has been initialized, but
		///     before the model has been locked down and used to initialize the context.  The default
		///     implementation of this method does nothing, but it can be overridden in a derived class
		///     such that the model can be further configured before it is locked down.
		/// </summary>
		/// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
		/// <remarks>
		///     Typically, this method is called only once when the first instance of a derived context
		///     is created.  The model for that context is then cached and is for all further instances of
		///     the context in the app domain.  This caching can be disabled by setting the ModelCaching
		///     property on the given ModelBuidler, but note that this can seriously degrade performance.
		///     More control over caching is provided through use of the DbModelBuilder and DbContextFactory
		///     classes directly. This method cannot be overidden. Override CreateMethod instead
		/// </remarks>
		protected override sealed void OnModelCreating(DbModelBuilder modelBuilder)
		{
			this.CreateModel(modelBuilder);
		}

		/// <summary>
		/// Called by sealed OnModelCreating to let derived classes provide their own implemenation
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected virtual void CreateModel(DbModelBuilder modelBuilder)
		{
            var models = this.CustomConfiguration.ModelsFactory != null ? this.CustomConfiguration.ModelsFactory() : ReflectionHelper.GetAllTypes()
				.Where(t => !t.IsAbstract && !t.IsInterface && typeof(BaseModel).IsAssignableFrom(t));

			foreach(var m in models)
			{
				modelBuilder.RegisterEntityType(m);
			}
			
			modelBuilder.Types()
				.Configure(config => { config.ToTable(config.ClrType.Name); });

			//foreach (var map in MappingConfiguration.Mappings)
			//	map.AddToConfigurationRegistrar(modelBuilder.Configurations);
		}

		/// <summary>
		/// Adds the specified item.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		T IDbContext.Add<T>(T item)
		{
			return base.Set<T>().Add(item);
		}

		/// <summary>
		/// Removes the specified item.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		bool IDbContext.Remove<T>(T item)
		{
			return base.Set<T>().Remove(item) != null;
		}
		/// <summary>
		/// Sets this instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		IQueryable<T> IDbContext.Set<T>()
		{
			return base.Set<T>();
		}

		/// <summary>
		/// Saves all changes made in this context to the underlying database.
		/// </summary>
		/// <returns>
		/// The number of state entries written to the underlying database. This can include
		/// state entries for entities and/or relationships. Relationship state entries are created for
		/// many-to-many relationships and relationships where there is no foreign key property
		/// included in the entity class (often referred to as independent associations).
		/// </returns>
		int IDbContext.SaveChanges()
		{
			return base.SaveChanges();
		}
	}
}