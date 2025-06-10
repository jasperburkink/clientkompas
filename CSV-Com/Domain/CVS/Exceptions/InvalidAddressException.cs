using Domain.CVS.ValueObjects;

namespace Domain.CVS.Exceptions
{
    public class InvalidAddressException(Address address) : Exception($"Address \"{address.GetFormattedAddress()}\" is invalid.")
    {
    }
}
