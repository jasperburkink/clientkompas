namespace Domain.Constants
{
    public static class ClientConstants
    {
        public const int ClientFirstNameMaxLength = 50;
        public const int ClientLastNameMaxLength = 50;
        public const int ClientPrefixLastNameMaxLength = 15;
        public const int ClientInitialsMaxLength = 15;
        public const int ClientFullnameMaxLength = 120;
        public const int ClientStreetnameMaxLength = AddressConstants.StreetNameMaxLength;
        public const int ClientPostalcodeMaxLength = AddressConstants.PostalCodeMaxLength;
        public const int ClientHouseNumberMaxLength = AddressConstants.HouseNumberMaxLength;
        public const int ClientHouseNumberAdditionMaxLength = AddressConstants.HouseNumberAdditionMaxLength;
        public const int ClientResidenceMaxLength = AddressConstants.ResidenceMaxLength;
        public const int ClientTelephoneNumberMaxLength = 15;
        public const int ClientEmailAddressMaxLength = 250;
        public const int ClientRemarksMaxLength = 1000;
        public const int ClientEmergencyPeopleNameMaxLength = 90;
        public const int CLientEmergencyPeopleTelephoneNumberMaxLength = 15;
    }
}
