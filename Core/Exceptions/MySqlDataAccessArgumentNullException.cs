namespace velocist.MySqlDataAccess.Core.Exceptions {
	public class MySqlDataAccessArgumentNullException : ArgumentNullException {
		public MySqlDataAccessArgumentNullException() {
		}

		public MySqlDataAccessArgumentNullException(string? paramName, string? message) : base(paramName, message) {
		}
	}

	public class MySqlDataAccessNotImplementException : NotImplementedException {

		public MySqlDataAccessNotImplementException() {
			throw new MySqlDataAccessShouldNotImplementException("This feature is not yet implemented");
		}

		public MySqlDataAccessNotImplementException(string? message) : base(message) {
		}
	}

	public class MySqlDataAccessShouldNotImplementException : NotImplementedException {

		public MySqlDataAccessShouldNotImplementException() {
			throw new MySqlDataAccessShouldNotImplementException("This function should not be implemented in this service.");
		}

		public MySqlDataAccessShouldNotImplementException(string? message) : base(message) {
		}
	}
}
