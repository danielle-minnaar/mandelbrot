namespace Mandelbrot.src.Exceptions;


/// <summary>
///     Represents errors that occur as a result of attempting
///     to call a service that is already in operation
///     and is not supposed to be active on more than one
///     operation at a time.
/// </summary>
[Serializable]
public class InOperationException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="InOperationException"/>
    ///     class.
    /// </summary>
    public InOperationException() { }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="InOperationException"/>
    ///     class with a specified error message.
    /// </summary>
    /// <param name="message">
    ///     The message held by this exception.
    /// </param>
    public InOperationException(string message)
        : base(message) { }
    

    /// <summary>
    ///     Initializes a new instance of the <see cref="InOperationException"/>
    ///     class with a specified error message and a reference to
    ///     the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">
    ///     The message held by this exception.
    /// </param>
    /// <param name="inner">
    ///     The inner exception that caused this exception.
    /// </param>
    public InOperationException(string message, Exception inner)
        : base(message, inner) { }
}