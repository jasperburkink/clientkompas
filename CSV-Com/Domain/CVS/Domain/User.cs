using System.Text.RegularExpressions;
using Domain.Common;

namespace Domain.CVS.Domain
{
    public class User : BaseAuditableEntity
    {
        public required string FirstName { get; set; }

        public string? PrefixLastName { get; set; }

        public required string LastName { get; set; }

        public string FullName
        {
            get
            {
                var fullName = string.Join(' ', (new string[] { FirstName, PrefixLastName, LastName })
                .Where(fv => !string.IsNullOrEmpty(fv))
                .Select(s => s.Trim()));

                return Regex.Replace(fullName, @"\s+", " ");
            }
            set => _ = value;
        }

        public required string EmailAddress { get; set; }

        public required string TelephoneNumber { get; set; }

        public required bool IsDeactivated { get; set; }

        public User? CreatedByUser { get; set; }

        public int? CreatedByUserId { get; set; }
    }
}
