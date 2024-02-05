using Domain.CVS.ValueObjects;

namespace Domain.CVS.Exceptions
{
    public class InvalidAddressException : Exception
    {
        public InvalidAddressException(Address address) : base($"Address \"{address.GetFormattedAddress()}\" is invalid.")
        {
        }
    }
}
