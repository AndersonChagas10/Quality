using System;

[Serializable()]
public class ExceptionHelper : System.Exception
{
    public ExceptionHelper() : base() { }
    public ExceptionHelper(string message) : base(message) { }
    public ExceptionHelper(string message, System.Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client. 
    protected ExceptionHelper(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
    { }
}