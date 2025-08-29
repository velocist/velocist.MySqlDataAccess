
namespace velocist.MySqlDataAccess;
/// <summary>
/// Unit of Work implementation for MySQL connections.
/// </summary>
public class UnitOfWorkMySql : IBaseUnitOfWork {

	private readonly MySqlConnector _connector;

	/// <summary>
	/// Constructor of the unit of work class
	/// </summary>
	/// <param name="connector">Connector of the connection</param>
	public UnitOfWorkMySql(IBaseConnector connector) {
		_connector = (MySqlConnector)connector;
	}

	/// <inheritdoc/>
	public void Connect() {
		if (_connector == null)
			throw new MySqlDataAccessArgumentNullException(nameof(_connector), ErrorCodesId.NullConnection.ToDescription());

		_connector.Open();
	}

	/// <inheritdoc/>
	public void BeginTransaction() {
		if (_connector == null)
			throw new MySqlDataAccessArgumentNullException(nameof(_connector), ErrorCodesId.NullConnection.ToDescription());

		_connector.BeginTransaction();
	}

	/// <inheritdoc/>
	public void Commit() {
		if (_connector == null)
			throw new MySqlDataAccessArgumentNullException(nameof(_connector), ErrorCodesId.NullConnection.ToDescription());

		_connector.Commit();
	}

	/// <inheritdoc/>
	public void Rollback() {
		if (_connector == null)
			throw new MySqlDataAccessArgumentNullException(nameof(_connector), ErrorCodesId.NullConnection.ToDescription());

		_connector.Rollback();
	}

	/// <inheritdoc/>
	public string Read(string sql) => _connector == null
			? throw new MySqlDataAccessArgumentNullException(nameof(_connector), ErrorCodesId.NullConnection.ToDescription())
			: _connector.ReadV4(sql);

	/// <inheritdoc/>
	public int ExecuteSql(string sql) => _connector == null
			? throw new MySqlDataAccessArgumentNullException(nameof(_connector), ErrorCodesId.NullConnection.ToDescription())
			: _connector.Execute(sql).Result;

	/// <inheritdoc/>
	public int Save() => throw new MySqlDataAccessShouldNotImplementException();
	/// <inheritdoc/>
	public bool CanConnect() => throw new MySqlDataAccessShouldNotImplementException();
	/// <inheritdoc/>
	public int ExecuteSql(string sql, object[] parameters) => throw new MySqlDataAccessShouldNotImplementException();
	//public int ExecuteSql(FormattableString sql) => throw new MySqlDataAccessNotImplementException();
	/// <inheritdoc/>
	public IEnumerable<TEntity> ExecuteSql<TEntity>(string sql) => throw new MySqlDataAccessShouldNotImplementException();

	/// <inheritdoc/>
	public void Dispose() {
		Trace.Write("Dispose UnitOfWorkMySql");
	}
}
