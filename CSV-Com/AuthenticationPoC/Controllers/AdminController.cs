using AuthenticationPoC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationPoC.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;

        public AdminController(UserManager<AppUser> usrMgr)
        {
            userManager = usrMgr;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Gebruiker gebruiker)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser
                {
                    UserName = gebruiker.Gebruikersnaam,
                    Email = gebruiker.Email                    
                };

                IdentityResult result = await userManager.CreateAsync(appUser, gebruiker.Wachtwoord);

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(gebruiker);
        }
    }
}
