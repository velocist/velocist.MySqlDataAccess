using velocist.Services.Core.Interfaces.MySql;


namespace velocist.MySqlDataAccess {
	public class UnitOfWorkMySql : IBaseUnitOfWork {

		//private static ILogger<UnitOfWork> _logger;
		private readonly MySqlConnector? _connector;

		/// <summary>
		/// Constructor of the unit of work class
		/// </summary>
		/// <param name="connector">Connector of the connection</param>
		public UnitOfWorkMySql(IBaseConnector connector) {
			_connector = (MySqlConnector?)connector;
			//_logger = GetStaticLogger<UnitOfWork>();
			//_logger = LogService.LogServiceContainer.GetLog<UnitOfWork>();
			//_logger.LogTrace("{unitOfWorkType} {connectorType}", typeof(UnitOfWork).Name, typeof(IConnector).Name);
		}

		/// <inheritdoc/>
		public void Connect() {
			if (_connector == null)
				throw new ArgumentNullException(nameof(_connector), ErrorCodesId.NullConnection.ToDescription());

			_connector.Open();
		}

		/// <inheritdoc/>
		public void BeginTransaction() {
			if (_connector == null)
				throw new ArgumentNullException(nameof(_connector), ErrorCodesId.NullConnection.ToDescription());

			_connector.BeginTransaction();
		}

		/// <inheritdoc/>
		public void Commit() {
			if (_connector == null)
				throw new ArgumentNullException(nameof(_connector), ErrorCodesId.NullConnection.ToDescription());

			_connector.Commit();
		}

		/// <inheritdoc/>
		public void Rollback() {
			if (_connector == null)
				throw new ArgumentNullException(nameof(_connector), ErrorCodesId.NullConnection.ToDescription());

			_connector.Rollback();
		}

		/// <inheritdoc/>
		public string? Read(string sql) => _connector == null
				? throw new ArgumentNullException(nameof(_connector), ErrorCodesId.NullConnection.ToDescription())
				: _connector.ReadV4(sql);

		/// <inheritdoc/>
		public int ExecuteSql(string sql) => _connector == null
				? throw new ArgumentNullException(nameof(_connector), ErrorCodesId.NullConnection.ToDescription())
				: _connector.Execute(sql).Result;

		public int Save() => throw new NotImplementedException();
		public bool CanConnect() => throw new NotImplementedException();
		public int ExecuteSql(string sql, object[] parameters) => throw new NotImplementedException();
		public int ExecuteSql(FormattableString sql) => throw new NotImplementedException();
		public IEnumerable<TEntity> ExecuteSql<TEntity>(string sql) => throw new NotImplementedException();
		public void Dispose() {
			//throw new NotImplementedException();
		}
	}
}
