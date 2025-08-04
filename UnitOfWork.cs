namespace velocist.MySqlDataAccess {

	/// <summary>
	/// Unit of work class
	/// </summary>
	public class UnitOfWork : UnitOfWorkMySql {
		public UnitOfWork(IBaseConnector connector) : base(connector) {
		}
	}
}
