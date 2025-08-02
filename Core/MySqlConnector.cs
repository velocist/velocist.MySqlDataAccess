using velocist.Services.Core;
using velocist.Services.Core.Interfaces.MySql;

namespace velocist.MySqlDataAccess.Core {

	/// <summary>
	/// Class for connection with MySQL
	/// </summary>
	public class MySqlConnector : IBaseConnector {

		/// <summary>
		/// The logger
		/// </summary>
		private static ILogger _logger;

		/// <summary>
		/// The database connection
		/// </summary>
		private readonly MySqlConnection _databaseConnection;

		/// <summary>
		/// Info of the connection
		/// </summary>
		public InfoConnection ConnectionInformation { get; set; }

		/// <summary>
		/// Transaction of the connection
		/// </summary>
		public MySqlTransaction Transaction { get; set; }

		/// <summary>
		/// Constructor for MySqlConnector
		/// </summary>
		/// <param name="connectionString"></param>
		public MySqlConnector(string connectionString) {
			try {
				//_logger = GetStaticLogger<MySqlConnector>();
				_logger = GetStaticLogger<MySqlConnector>();

				if (!string.IsNullOrEmpty(connectionString)) {
					_databaseConnection = new MySqlConnection(connectionString);

					ConnectionInformation = new InfoConnection {
						ClientId = Guid.NewGuid(),
						ConnectionString = _databaseConnection.ConnectionString,
						Database = _databaseConnection.Database,
						DataSource = _databaseConnection.DataSource,
						Timeout = _databaseConnection.ConnectionTimeout
					};
					//_logger.LogDebug("{clientId} {server}/{database}", ConnectionInformation.ClientId.ToString(), ConnectionInformation.DataSource, ConnectionInformation.DatabaseService);
				} else
					throw new ArgumentNullException(connectionString);
			} catch (Exception ex) {
				ErrorCollector.AddError(nameof(MySqlConnector), "Constructor", ex);
			}
		}

		///<inheritDoc/>
		public void Close() {
			try {
				if (_databaseConnection == null)
					throw new ArgumentNullException(nameof(_databaseConnection));

				if (_databaseConnection.State.Equals(ConnectionState.Open))
					_databaseConnection.Close();
				else {
					//WriteLogConnectionInfo(LogLevel.Trace, message: $"CONNECTION STATE: {_databaseConnection.State}");
				}
			} catch (Exception ex) {
				ErrorCollector.AddError(nameof(MySqlConnector), nameof(Close), ex);
			}
		}

		///<inheritDoc/>
		public async Task<int> Execute(string sql) {
			try {
				if (_databaseConnection.State != ConnectionState.Open)
					Open();

				using MySqlCommand commandDatabase = new(sql, _databaseConnection);
				var result = await commandDatabase.ExecuteNonQueryAsync();

				WriteLogConnectionInfo(LogLevel.Trace, message: $"EXECUTE {result} registro/s afectados. SQL: {sql}");

				if (result == 1)
					if (sql.Contains("INSERT")) {
						var lastInsert = (int)commandDatabase.LastInsertedId;
						return lastInsert > 0 ? (int)commandDatabase.LastInsertedId : 1;
					}

				return result;
			} catch (Exception ex) {
				ErrorCollector.AddError(nameof(MySqlConnector), nameof(Execute), ex);
				throw;
			}
		}

		///<inheritDoc/>
		public void Open() {
			try {
				if (_databaseConnection == null)
					throw new ArgumentNullException(nameof(_databaseConnection));

				if (_databaseConnection.State.Equals(ConnectionState.Closed))
					_databaseConnection.Open();
				else {
					//WriteLogConnectionInfo(LogLevel.Trace, message: $"CONNECTION WAS ALREADY OPEN. STATE: {_databaseConnection.State}");
				}
			} catch (Exception ex) {
				ErrorCollector.AddError(nameof(MySqlConnector), nameof(Open), ex);
				throw;
			}
		}

		///<inheritDoc/>
		public string? ReadV4(string sql) {
			try {
				if (_databaseConnection.State != ConnectionState.Open)
					Open();

				DataTable? dt = null;
				using (MySqlCommand commandDatabase = new(sql, _databaseConnection)) {
					using MySqlDataAdapter da = new(commandDatabase);
					dt = new DataTable();
					_ = da.Fill(dt);
					if (dt != null) {
						var result = dt.Rows.Count;

						WriteLogConnectionInfo(LogLevel.Trace, message: $"READ {result} resultados. SQL: {sql}");

						if (result > 0) {
							var serializeString = JsonHelper<DataTable>.Serialize(dt);
							return serializeString;
						}
					}
				}

				return null;
			} catch (Exception ex) {
				ErrorCollector.AddError(nameof(MySqlConnector), nameof(ReadV4), ex);
				throw;
			}
		}

		///<inheritDoc/>
		public void BeginTransaction() {
			if (_databaseConnection == null)
				throw new ArgumentNullException(nameof(_databaseConnection));

			Transaction = _databaseConnection.BeginTransaction();
			WriteLogConnectionInfo(LogLevel.Trace, message: "BEGIN TRANSACTION.");
		}

		///<inheritDoc/>
		public void Commit() {
			//try {
			if (_databaseConnection == null)
				throw new ArgumentNullException(nameof(_databaseConnection));

			Transaction.Commit();
			WriteLogConnectionInfo(LogLevel.Trace, message: "COMMIT TRANSACTION.");
			//} catch (Exception ex) {
			//    WriteLogConnectionInfo(LogLevel.Error, message: "ERROR ON COMMIT TRANSACTION.");
			//    Close();
			//}
		}

		///<inheritDoc/>
		public void Rollback() {
			if (_databaseConnection == null)
				throw new ArgumentNullException(nameof(_databaseConnection));

			Transaction.Rollback();
			WriteLogConnectionInfo(LogLevel.Trace, message: "ROLLBACK TRANSACTION.");
		}

		///<inheritDoc/>
		public void Save() {
			if (_databaseConnection == null)
				throw new ArgumentNullException(nameof(_databaseConnection));

			Transaction.Save($"{ConnectionInformation.ClientId}");
			WriteLogConnectionInfo(LogLevel.Trace, message: "SAVE TRANSACTION.");

		}

		/// <summary>
		/// Dispose the connector
		/// </summary>
		public void Dispose() {
			try {
				if (_databaseConnection == null)
					throw new ArgumentNullException(nameof(_databaseConnection));

				_databaseConnection.Close();
				_databaseConnection.Dispose();
				//WriteLogConnectionInfo(LogLevel.Trace, message: "CONNECTION HAS DISPOSE.");
			} catch (Exception ex) {
				ErrorCollector.AddError(nameof(MySqlConnector), nameof(Dispose), ex);
			}
		}

		/// <summary>
		/// Writes the log connection information.
		/// </summary>
		/// <param name="logLevel">The log level.</param>
		/// <param name="ex">The ex.</param>
		/// <param name="message">The message.</param>
		private void WriteLogConnectionInfo(LogLevel logLevel, Exception? ex = null, string message = "") {

			var messageFormat = "{0} DataSource: {1} DatabaseService: {2} {3}";
			object[] messageParams = new[] { ConnectionInformation.ClientId.ToString(), ConnectionInformation.DataSource, ConnectionInformation.Database, message };

			if (LogLevel.Error == logLevel)
				_logger.LogError(ex, messageFormat, messageParams);

			else if (LogLevel.Trace == logLevel)
				_logger.LogTrace(messageFormat, messageParams);

			else if (LogLevel.Debug == logLevel)
				_logger.LogDebug(messageFormat, messageParams);

			else if (LogLevel.Information == logLevel)
				_logger.LogInformation(messageFormat, messageParams);

			else if (LogLevel.Warning == logLevel)
				_logger.LogWarning(messageFormat, messageParams);

			else if (LogLevel.Error == logLevel)
				_logger.LogError(messageFormat, messageParams);

			else if (LogLevel.Critical == logLevel)
				_logger.LogCritical(messageFormat, messageParams);
		}
	}
}