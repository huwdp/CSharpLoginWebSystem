using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LoginExample.WebApp.Models;
using LoginExample.WebApp.Store;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LoginExample.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private IUserStore _userStore;

        public UserModel CurrentUserModel
        {
            get {
                string userId = HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
                return _userStore.getById(Int32.Parse(userId));
            }
        }

        public HomeController(IUserStore userStore)
        {
            _userStore = userStore;
        }

        [AllowAnonymous]
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Register(Models.UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                _userStore.registerUser(userModel.Username, userModel.Password);
                return RedirectToAction("Login");
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(Models.UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                UserModel validationUser = _userStore.loginUser(userModel.Username, userModel.Password);
                if (validationUser != null)
                {
                    // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-2.2
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, validationUser.Username));
                    identity.AddClaim(new Claim(ClaimTypes.Sid, validationUser.Id.ToString()));
                    var principal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).GetAwaiter();
                    return RedirectToAction("View");
                }
                return View();
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            UserModel userModel = _userStore.getById(id);

            if (ModelState.IsValid)
            {
                _userStore.save(userModel);

                return View();
            }
            return View();
        }

        [Authorize]
        public IActionResult View(int id)
        {
            return View(CurrentUserModel);
        }
    }
}
