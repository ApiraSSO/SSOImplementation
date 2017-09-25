using System;
using Kernel.Data;

namespace Kernel.Data
{
	public abstract class BaseModel : IHasID<Guid>, IEquatable<BaseModel>
	{
		#region static method

		/// <summary>
		///     Overrides == operator to compare models on their IDs
		/// </summary>
		/// <param name="model1"></param>
		/// <param name="model2"></param>
		/// <returns></returns>
		public static bool operator ==(BaseModel model1, BaseModel model2)
		{
			if (((object)model1) == null || ((object)model2) == null)
				return Object.Equals(model1, model2);

			return model1.Equals(model2);
		}

		/// <summary>
		///     Overrides != operatior to compare models on their IDs
		/// </summary>
		/// <param name="model1"></param>
		/// <param name="model2"></param>
		/// <returns></returns>
		public static bool operator !=(BaseModel model1, BaseModel model2)
		{
			if (((object)model1) == null || ((object)model2) == null)
				return !Object.Equals(model1, model2);

			return !(model1.Equals(model2));
		}

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseModel"/> class.
		/// </summary>
		protected BaseModel()
		{
			this.Created = DateTimeOffset.Now;
			this.Id = Guid.NewGuid();
			this.IsDeleted = false;
		}

		#endregion

		#region properties

		/// <summary>
		///     Gets or sets the date the object was created.
		/// </summary>
		/// <value>
		///     The created.
		/// </value>
		public DateTimeOffset Created { get; set; }

		/// <summary>
		///     Gets or sets the identifier.
		/// </summary>
		/// <value>
		///     The identifier.
		/// </value>
		public Guid Id { get; set; }

		/// <summary>
		///     Gets or sets a value indicating whether this instance is deleted.
		/// </summary>
		/// <value>
		///     <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
		/// </value>
		public bool IsDeleted { get; set; }

		/// <summary>
		///     Gets or sets the last updated.
		/// </summary>
		/// <value>
		///     The last updated.
		/// </value>
		public DateTimeOffset LastUpdated { get; set; }

		/// <summary>
		/// Gets or sets the row version.
		/// </summary>
		/// <value>
		/// The row version.
		/// </value>
		public byte[] RowVersion { get; set; }

		#endregion

		#region Methods

		/// <summary>
		///     IEquatable<BaseModel> implementation. Overrides Equals to compare models on Ids(GUID) .
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(BaseModel other)
		{
			if (other == null)
				return false;

			return this.Id == other.Id;
		}

		/// <summary>
		///     Overrides Object.Equals to compare models on IDS
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var otherModel = obj as BaseModel;

			if (otherModel == null)
				return false;
			return this.Equals(otherModel);
		}

		/// <summary>
		///     Get hash code of the model. Computed by GUID hash code
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}

		#endregion
	}
}