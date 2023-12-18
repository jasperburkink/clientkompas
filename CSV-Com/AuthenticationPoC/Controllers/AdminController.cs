using AuthenticationPoC.Models;
using AuthenticationPoC.ViewModels;
using CVSInfrastructurePoC.Repositories;
using CVSModelPoC;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationPoC.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IPasswordValidator<AppUser> _passwordValidator;
        private readonly IUserValidator<AppUser> _userValidator;
        private readonly IGebruikerRepository _gebruikerRepository;

        public AdminController(UserManager<AppUser> usrMgr, IPasswordHasher<AppUser> passwordHash, IPasswordValidator<AppUser> passwordVal, IUserValidator<AppUser> userValid, IGebruikerRepository gebruikerRepository)
        {
            _userManager = usrMgr;
            _passwordHasher = passwordHash;
            _passwordValidator = passwordVal;
            _userValidator = userValid;
            _gebruikerRepository = gebruikerRepository;
        }

        public IActionResult Index()
        {
            return View(_userManager.Users);
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
                await _gebruikerRepository.InsertGebruikerAsync(cvsGebruiker);
                await _gebruikerRepository.SaveAsync();
                cvsGebruiker = await _gebruikerRepository.GetGebruikerByEmailAsync(cvsGebruiker.Email);

                // Authentication gebruiker
                var appUser = new AppUser
                {
                    CVSUserId = cvsGebruiker.Id,
                    UserName = gebruiker.Gebruikersnaam,
                    Email = gebruiker.Email,
                    TwoFactorEnabled = true
                };
                var result = await _userManager.CreateAsync(appUser, gebruiker.Wachtwoord);

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(gebruiker);
        }

        public async Task<IActionResult> Update(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var cvsGebruiker = await _gebruikerRepository.GetGebruikerAsync(user.CVSUserId);

            if (user == null || cvsGebruiker == null)
            {
                ModelState.AddModelError("", "User Not Found"); // NOTE: heeft dit zin als je daarna redirect naar index?
            }
            else
            {
                var gebruikerViewModel = new GebruikerViewModel
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
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.TwoFactorEnabled = true;

                IdentityResult validEmail = null;
                if (!string.IsNullOrEmpty(email))
                {
                    validEmail = await _userValidator.ValidateAsync(_userManager, user);
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
                    validPass = await _passwordValidator.ValidateAsync(_userManager, user, password);
                    if (validPass.Succeeded)
                        user.PasswordHash = _passwordHasher.HashPassword(user, password);
                    else
                        Errors(validPass);
                }
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                if (validEmail != null && validPass != null && validEmail.Succeeded && validPass.Succeeded)
                {
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }

                var cvsUser = await _gebruikerRepository.GetGebruikerAsync(user.CVSUserId);

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

                    await _gebruikerRepository.UpdateGebruikerAsync(cvsUser);
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("Index", _userManager.Users);
        }

        private void Errors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
