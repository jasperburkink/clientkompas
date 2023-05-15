using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Authentication.Domain
{
    public class AuthenticationUser : IdentityUser
    {
        public int CVSUserId { get; set; }
    }
}
