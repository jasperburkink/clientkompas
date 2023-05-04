using AuthenticationPoC.Models;
using AuthenticationPoC.ViewModels;
using CVSInfrastructurePoC.Repositories;
using CVSModelPoC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Data;

namespace AuthenticationPoC.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;
        private IPasswordHasher<AppUser> passwordHasher;
        private IPasswordValidator<AppUser> passwordValidator;
        private IUserValidator<AppUser> userValidator;
        private readonly IGebruikerRepository gebruikerRepository;

        public AdminController(UserManager<AppUser> usrMgr, IPasswordHasher<AppUser> passwordHash, IPasswordValidator<AppUser> passwordVal, IUserValidator<AppUser> userValid, IGebruikerRepository gebruikerRepository)
        {
            userManager = usrMgr;
            passwordHasher = passwordHash;
            passwordValidator = passwordVal;
            userValidator = userValid;
            this.gebruikerRepository = gebruikerRepository;
        }
        
        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(GebruikerViewModel gebruiker)
        {
            if (ModelState.IsValid)
            {
                // CVS data
                var cvsGebruiker = new Gebruiker()
                {
                    Voornaam = gebruiker.Voornaam,
                    Achternaam = gebruiker.Achternaam,
                    Email = gebruiker.Email
                };
                await gebruikerRepository.InsertGebruikerAsync(cvsGebruiker);
                await gebruikerRepository.SaveAsync();
                cvsGebruiker =  await gebruikerRepository.GetGebruikerByEmailAsync(cvsGebruiker.Email);

                // Authentication gebruiker
                AppUser appUser = new AppUser
                {
                    CVSUserId = cvsGebruiker.Id,
                    UserName = gebruiker.Gebruikersnaam,
                    Email = gebruiker.Email,
                    TwoFactorEnabled = true
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

        public async Task<IActionResult> Update(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            Gebruiker cvsGebruiker = await gebruikerRepository.GetGebruikerAsync(user.CVSUserId);

            if (user == null || cvsGebruiker == null)
            {
                ModelState.AddModelError("", "User Not Found"); // NOTE: heeft dit zin als je daarna redirect naar index?
            }
            else
            {
                GebruikerViewModel gebruikerViewModel = new GebruikerViewModel
                {
                    Id = user.Id,
                    Achternaam = cvsGebruiker.Achternaam,
                    Voornaam = cvsGebruiker.Voornaam,
                    Gebruikersnaam = user.UserName,
                    Email = user.Email
                };

                return View(gebruikerViewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, string email, string voornaam, string achternaam, string password)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.TwoFactorEnabled = true;

                IdentityResult validEmail = null;
                if (!string.IsNullOrEmpty(email))
                {
                    validEmail = await userValidator.ValidateAsync(userManager, user);
                    if (validEmail.Succeeded)
                        user.Email = email;
                    else
                        Errors(validEmail);
                }
                else
                    ModelState.AddModelError("", "Email cannot be empty");

                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, user, password);
                    if (validPass.Succeeded)
                        user.PasswordHash = passwordHasher.HashPassword(user, password);
                    else
                        Errors(validPass);
                }
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                if (validEmail != null && validPass != null && validEmail.Succeeded && validPass.Succeeded)
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }

                var cvsUser = await gebruikerRepository.GetGebruikerAsync(user.CVSUserId);

                if (cvsUser == null)
                {
                    ModelState.AddModelError("", "CVSUser Not Found");
                    return View(user);
                }
                else
                {
                    cvsUser.Email = email;
                    cvsUser.Achternaam = achternaam;
                    cvsUser.Voornaam = voornaam;

                    await gebruikerRepository.UpdateGebruikerAsync(cvsUser);
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("Index", userManager.Users);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
