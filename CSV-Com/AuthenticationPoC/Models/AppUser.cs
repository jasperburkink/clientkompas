using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace AuthenticationPoC.Models
{
    public class AppUser : IdentityUser
    {
        public int CVSUserId { get; set; }
    }
}
