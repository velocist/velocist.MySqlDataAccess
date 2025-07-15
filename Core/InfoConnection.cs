namespace velocist.MySqlDataAccess.Core {

	/// <summary>
	/// Info fot the connection
	/// </summary>
	public class InfoConnection {

		///// <summary>
		///// Host of the request connection
		///// </summary>
		//public string Host { get; set; }

		///// <summary>
		///// Method of the request connection
		///// </summary>
		//public string Method { get; set; }

		///// <summary>
		///// Path of the request connection
		///// </summary>
		//public string PathValue { get; set; }

		/// <summary>
		/// Client id of the request connection
		/// </summary>
		public Guid ClientId { get; set; }

		/// <summary>
		/// Gets or sets the connection string.
		/// </summary>
		/// <value>
		/// The connection string.
		/// </value>
		public string ConnectionString { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the data source.
		/// </summary>
		/// <value>
		/// The data source.
		/// </value>
		public string DataSource { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the database.
		/// </summary>
		/// <value>
		/// The database.
		/// </value>
		public string Database { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the timeout.
		/// </summary>
		/// <value>
		/// The timeout.
		/// </value>
		public int Timeout { get; set; }
	}
}
