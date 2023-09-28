namespace ExMath.Exceptions
{
	/// <summary>
	/// Represents errors that occurs when a type parameter is not detected to be numerical.
	/// </summary>
	[Serializable]
	public class InvalidNumericTypeException : Exception
	{
		public InvalidNumericTypeException() { }
		public InvalidNumericTypeException(string message) : base(message) { }
		public InvalidNumericTypeException(string message, Exception inner) : base(message, inner) { }
		protected InvalidNumericTypeException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
