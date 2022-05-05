namespace HotpServer.Exceptions
{

    [Serializable]
    public class NoSuchUserException : Exception
    {
        public NoSuchUserException() { }
        public NoSuchUserException(string message) : base(message) { }
        public NoSuchUserException(string message, Exception inner) : base(message, inner) { }
        protected NoSuchUserException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
