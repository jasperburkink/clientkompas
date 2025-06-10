namespace Application.Common.Exceptions
{
    public class ItemAlreadyExistsException : Exception
    {
        public ItemAlreadyExistsException()
            : base()
        {
        }

        public ItemAlreadyExistsException(string message)
            : base(message)
        {
        }

        public ItemAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
