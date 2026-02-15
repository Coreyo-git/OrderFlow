namespace OrderService.Domain.Exceptions;

/// <summary>
/// Represents errors that occur during order service domain logic execution.
/// </summary>
public class DomainException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="DomainException"/> class.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public DomainException(string message) : base(message)
	{
	}
}