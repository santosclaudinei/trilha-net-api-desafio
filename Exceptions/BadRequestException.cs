namespace TrilhaApiDesafio.Exceptions;

public class BadRequestException : Exception
{
	public BadRequestException(string errorMessage) : base(errorMessage) { }
}