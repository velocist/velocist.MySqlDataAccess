namespace velocist.MySqlDataAccess.Core.Exceptions;
/// <summary>
/// Represents an exception that is thrown when a null argument is passed to a method in the MySqlDataAccess layer.
/// Inherits from <see cref="ArgumentNullException"/>.
/// </summary>
public class MySqlDataAccessArgumentNullException : ArgumentNullException {
	/// <summary>
	/// Initializes a new instance of the <see cref="MySqlDataAccessArgumentNullException"/> class.
	/// </summary>
	public MySqlDataAccessArgumentNullException() {
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="MySqlDataAccessArgumentNullException"/> class with a specified parameter name and error message.
	/// </summary>
	/// <param name="paramName">The name of the parameter that caused the exception.</param>
	/// <param name="message">The message that describes the error.</param>
	public MySqlDataAccessArgumentNullException(string paramName, string message) : base(paramName, message) {
	}
}

/// <summary>
/// Represents an exception that is thrown when a feature is not yet implemented in the MySqlDataAccess layer.
/// Inherits from <see cref="NotImplementedException"/>.
/// </summary>
public class MySqlDataAccessNotImplementException : NotImplementedException {
	/// <summary>
	/// Initializes a new instance of the <see cref="MySqlDataAccessNotImplementException"/> class.
	/// Throws a <see cref="MySqlDataAccessShouldNotImplementException"/> indicating the feature is not implemented.
	/// </summary>
	public MySqlDataAccessNotImplementException() {
		throw new MySqlDataAccessShouldNotImplementException("This feature is not yet implemented");
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="MySqlDataAccessNotImplementException"/> class with a specified error message.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public MySqlDataAccessNotImplementException(string message) : base(message) {
	}
}

/// <summary>
/// Represents an exception that is thrown when a function should not be implemented in the MySqlDataAccess service.
/// Inherits from <see cref="NotImplementedException"/>.
/// </summary>
public class MySqlDataAccessShouldNotImplementException : NotImplementedException {
	/// <summary>
	/// Initializes a new instance of the <see cref="MySqlDataAccessShouldNotImplementException"/> class.
	/// Throws itself with a default message indicating the function should not be implemented.
	/// </summary>
	public MySqlDataAccessShouldNotImplementException() {
		throw new MySqlDataAccessShouldNotImplementException("This function should not be implemented in this service.");
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="MySqlDataAccessShouldNotImplementException"/> class with a specified error message.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public MySqlDataAccessShouldNotImplementException(string message) : base(message) {
	}
}
