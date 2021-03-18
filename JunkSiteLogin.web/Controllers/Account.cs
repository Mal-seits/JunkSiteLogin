using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JunkSiteLogin.data;
using JunkSiteLogin.web.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace JunkSiteLogin.web.Controllers
{
    
    public class Account : Controller
    {
        private string _connectionString =
            "Data Source=.\\sqlexpress;Initial Catalog=JunkSiteLogin;Integrated Security=true;";
        public IActionResult Login()
        {
            LogInViewModel vm = new LogInViewModel();
            if (TempData["alert"] != null)
            {

                vm.Alert = (string)TempData["alert"];
                
            }
            
            return View(vm);
        }


        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            DbManager db = new DbManager(_connectionString);
            var user = db.LogIn(email, password);
            if(user == null)
            {
                TempData["Alert"] = "Incorrect Email/Password Combo, Please try again!";
                return Redirect("/account/login");
            }
            var claims = new List<Claim>
            {
                new Claim("user", email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();
            return Redirect("/home/index");
        }
        
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(string name, string email, string password)
        {
            var db = new DbManager(_connectionString);
            db.AddUser(name, email, password);
            return Redirect("/account/login");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/home/index");
        }
        public IActionResult MyAccount()
        {
            var db = new DbManager(_connectionString);
            var email = User.Identity.Name;
            var currentUser = db.GetByEmail(email);
            ViewMyAds vm = new ViewMyAds();
            vm.MyAds = db.GetAdsForUser(currentUser.Id);
            return View(vm);
        }
    }
}
