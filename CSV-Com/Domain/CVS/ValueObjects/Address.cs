using Domain.Common;
using Domain.CVS.Exceptions;

namespace Domain.CVS.ValueObjects
{
    public class Address : ValueObject
    {
        public string StreetName { get; } = "";

        public int HouseNumber { get; } = 0;

        public string HouseNumberAddition { get; } = "";

        public string PostalCode { get; } = "";

        public string Residence { get; } = "";

        static Address() { }

        internal Address()
        {
        }

        internal Address(string streetName, int houseNumber, string houseNumberAddition, string postalCode, string residence)
        {
            StreetName = streetName;
            HouseNumber = houseNumber;
            HouseNumberAddition = houseNumberAddition;
            PostalCode = postalCode;
            Residence = residence;
        }

        public static Address From(string streetName, int houseNumber, string houseNumberAddition, string postalCode, string residence)
        {
            var address = new Address(streetName, houseNumber, houseNumberAddition, postalCode, residence);

            if (!address.IsValid())
            {
                throw new InvalidAddressException(address);
            }

            return address;
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(StreetName) &&
                   HouseNumber > 0 &&
                   !string.IsNullOrWhiteSpace(PostalCode) &&
                   !string.IsNullOrWhiteSpace(Residence);
        }

        public string GetFormattedAddress()
        {
            var formattedAddress = $"{StreetName} {HouseNumber}";

            if (!string.IsNullOrWhiteSpace(HouseNumberAddition))
            {
                formattedAddress += $" {HouseNumberAddition}";
            }

            formattedAddress += $", {PostalCode} {Residence}";

            return formattedAddress;
        }

        public override string ToString()
        {
            return GetFormattedAddress();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StreetName;
            yield return HouseNumber;
            yield return HouseNumberAddition;
            yield return PostalCode;
            yield return Residence;
        }
    }
}
