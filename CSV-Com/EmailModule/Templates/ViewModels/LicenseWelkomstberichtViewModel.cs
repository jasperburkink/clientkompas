namespace OefenenDependancyInjection.EmailFolder.Templates.ViewModels
{
    public class LicenseWelkomstberichtViewModel
    {
        public string LicenseHolderName { get; set; }

        public string OrganizationName { get; set; }

        public string Status { get; set; }

        public DateOnly CreatedAt { get; set; }

        public DateOnly ValidUntil { get; set; }

    }


}
