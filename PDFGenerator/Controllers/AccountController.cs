using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PDFGenerator.Data;
using PDFGenerator.Models.AccountModels;
using PDFGenerator.Models.ClientModels;
using PDFGenerator.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger _logger;
        private readonly AppIdentityDbContext _idectx;
        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, 
            ILogger<AppUser> logger, AppIdentityDbContext idectx)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _idectx = idectx;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                TempData["Fail"] = "Jesteś już zalogowany.";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            var res = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, login.RememberMe, false);
            if (res.Succeeded)
            {
                TempData["Success"] = "Zostałeś zalogowany.";
                return RedirectToAction("Index", "Home");
            }
            TempData["Fail"] = "Niepoprawne dane logowania.";
            return View(login);
        }

        [HttpGet]
        public async  Task<IActionResult> Register()
        {
            var usrApp = await _userManager.GetUserAsync(User);
            if (!await _userManager.IsInRoleAsync(usrApp, "RCON"))
            {
                TempData["Fail"] = "Tylko szef może tworzyć nowe konta pracownicze";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.UserName, Email = model.Email, FirstName = model.FirstName,
                    SurName = model.SurName };
                //var userRole = new IdentityRole
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Employer");
                    _logger.LogInformation("User created a new account.");
                    TempData["Success"] = "Konto zostało stworzone!";
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            return View(model);
        }

        public async Task<IActionResult> List()
        {
            var usrApp = await _userManager.GetUserAsync(User);
            if (!await _userManager.IsInRoleAsync(usrApp, "RCON") && !await _userManager.IsInRoleAsync(usrApp, "Admin") &&
                !await _userManager.IsInRoleAsync(usrApp, "Employer"))
            {
                TempData["Fail"] = "Nie posiadasz uprawnień do tej podstrony";
                return RedirectToAction("Index", "Home");
            }
            return View(new AppUserViewModel
            {
                AppUsers = _idectx.Users
                    .OrderBy(p => p.UserName),
            });
        }

        [HttpPost]
        public async Task<IActionResult> SetRank(string NameOfUser)
        {
            var usrApp = await _userManager.GetUserAsync(User);
            if (!await _userManager.IsInRoleAsync(usrApp, "RCON"))
            {
                TempData["Fail"] = "Nie posiadasz uprawnień do tej podstrony";
                return RedirectToAction("Index", "Home");
            }
            var usr = await _userManager.FindByNameAsync(NameOfUser);
            return View(new AppUserViewModel
            {
                AppUser = usr
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(string NameOfUser, string Rank)
        {
            var usrApp = await _userManager.GetUserAsync(User);
            if (!await _userManager.IsInRoleAsync(usrApp, "RCON"))
            {
                TempData["Fail"] = "Nie posiadasz uprawnień do tej podstrony";
                return RedirectToAction("Index", "Home");
            }
            var usr = await _userManager.FindByNameAsync(NameOfUser);
            await _userManager.RemoveFromRolesAsync(usr, await _userManager.GetRolesAsync(usr));
            var changeRank = await _userManager.AddToRoleAsync(usr, Rank);
            if (changeRank.Succeeded)
            {
                TempData["Success"] = "Pomyślnie zmieniono rangę użytkownika";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Fail"] = "Nie udało zmienić się rangi użytkownika. Spróbuj ponownie";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Details(string NameOfUser)
        {
            var usrApp = await _userManager.GetUserAsync(User);
            if (!await _userManager.IsInRoleAsync(usrApp, "RCON") && !await _userManager.IsInRoleAsync(usrApp, "Admin") &&
                !await _userManager.IsInRoleAsync(usrApp, "Employer"))
            {
                TempData["Fail"] = "Nie posiadasz uprawnień do tej podstrony";
                return RedirectToAction("Index", "Home");
            }
            var usr = await _userManager.FindByNameAsync(NameOfUser);
            var usrRank = await _userManager.GetRolesAsync(usr);
            string rank = null;
            foreach (var r in usrRank)
            {
                rank = r;
            }
            return View(new AppUserViewModel
            {
                Rank = rank,
                AppUser = usr
            });
        }

        public IActionResult Logout()
        {
            if (_signInManager.IsSignedIn(User))
            {
                _signInManager.SignOutAsync();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
