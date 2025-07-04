﻿using AuthenticationPoC.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationPoC.ViewModels
{
    public class RoleEdit
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<AppUser> Members { get; set; }
        public IEnumerable<AppUser> NonMembers { get; set; }
    }
}