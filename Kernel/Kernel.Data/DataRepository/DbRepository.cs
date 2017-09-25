using System;
using System.Linq;
using System.Threading.Tasks;
using Kernel.Data.DataRepository;
using Kernel.Data.ORM;
using Kernel.Providers;

namespace Kernel.Data.Repository
{
    public class DbRepository<TEntity> : IDbRepository<TEntity>
		where TEntity : BaseModel, new()
	{
		#region fields
		private IDbContext context;
		private readonly IIdentitytProvider<Guid> IdentityProvider;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DbRepository{TEntity}"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="identityProvider">The identity provider.</param>
		/// <exception cref="System.ArgumentException"></exception>
		public DbRepository(IDbContext context, IIdentitytProvider<Guid> identityProvider)
		{
			this.context = context;
			this.IdentityProvider = identityProvider;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DbRepository{TEntity}"/> class.
		/// </summary>
		/// <param name="repo">The repo.</param>
		public DbRepository(IDbRepository repo)
		{
			this.context = repo.Context;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DbRepository{TEntity}"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public DbRepository(IDbContext context)
		{
			this.context = context;
		}

		#endregion

		IDbContext IDbRepository.Context
		{
			get { return this.context; }
		}

		#region Methods

		/// <summary>
		///     Creates the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>
		///     The ID of the newly created item, or null if creation was not successful
		/// </returns>
		public Task<Guid?> Create(TEntity item)
		{
			if (this.IdentityProvider == null)
				throw new ArgumentNullException("identityProvider");

			var result = this.context.Add(item);
			result.Id = this.IdentityProvider.GetId();

			return Task.FromResult<Guid?>(result.Id);
		}

		/// <summary>
		/// Reads the specified navigation properties.
		/// </summary>
		/// <param name="navigationProperties">The navigation properties.</param>
		/// <returns></returns>
		public IQueryable<TEntity> Read()
		{
			var dbQuery = this.context.Set<TEntity>();

			return dbQuery;
		}

		/// <summary>
		///     Reads all items.
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public async Task<TEntity> Read(Guid ID)
		{
			return await Task.Run(() =>
			{
				return this.Read().FirstOrDefault(x => ID == x.Id);
			});
		}

		/// <summary>
		/// Updates the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public Task<bool> Update(TEntity item)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		///     Deletes the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		/// <exception cref="System.InvalidOperationException"></exception>
		public Task<bool> Delete(TEntity item)
		{
			if (item == null)
				throw new InvalidOperationException(string.Format("Expected type: {0} but was: {1}", typeof(BaseModel).Name, item.GetType().Name));

			item.IsDeleted = true;

			this.context.Remove(item);

			return Task.FromResult(true);
		}

		/// <summary>
		///     Deletes the specified item.
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public async Task<bool> Delete(Guid ID)
		{
			var item = await this.Read(ID);
			return await this.Delete(item);
		}

		/// <summary>
		///     Non generic read by Id
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		
		Task<IHasID<Guid>> IRepository<Guid>.Read(Guid ID)
		{
			return this.Read(ID).ContinueWith(t => (IHasID<Guid>)t.Result);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Processes the batch by calling SaveChanges if it hasn't been called explicitly
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && context != null)
			{
				this.context.Dispose();
			}
		}

        public IQueryable<TEntity> ReadBatch(IQueryable<TEntity> query, int offset, int batchSize)
        {
            return query.Skip(offset)
                .Take(batchSize);
        }

        #endregion
    }
}