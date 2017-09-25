namespace Kernel.Data.Connection
{
	public interface IDbConnectionDefinition
	{
		/// <summary>
		///     Gets the database name.
		/// </summary>
		/// <value>
		///     The database.
		/// </value>
		string Database { get; }

		/// <summary>
		///     Gets the data source.
		/// </summary>
		/// <value>
		///     The data source.
		/// </value>
		string DataSource { get; }

		/// <summary>
		///     Gets the name of the user.
		/// </summary>
		/// <value>The name of the user.</value>
		string UserName { get; }

		/// <summary>
		///     Gets the password.
		/// </summary>
		/// <value>The password.</value>
		string Password { get; }

		/// <summary>
		///     Gets a value indicating whether this instance is integrated connection.
		/// </summary>
		/// <value><c>true</c> if this instance is integrated connection; otherwise, <c>false</c>.</value>
		bool IntegratedSecurity { get; }
	}
}