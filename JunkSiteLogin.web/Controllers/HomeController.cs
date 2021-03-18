using JunkSiteLogin.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using JunkSiteLogin.data;
using Microsoft.AspNetCore.Authorization;

namespace JunkSiteLogin.web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString =
             "Data Source=.\\sqlexpress;Initial Catalog=JunkSiteLogin;Integrated Security=true;";

        public IActionResult Index()
        {
            DbManager db = new DbManager(_connectionString);
            ViewAdsViewModel vm = new ViewAdsViewModel();
            vm.Ads = db.GetAllAds();
            if (User.Identity.IsAuthenticated)
            {
                var email = User.Identity.Name;
                vm.CurrentUser = db.GetByEmail(email);
            }
            return View(vm);
        }
        [Authorize]
        public IActionResult AddAd()
        {
            return View();
        }

        [HttpPost]
       public IActionResult AddAd(string phonenumber, string description)
        {
            var db = new DbManager(_connectionString);
            var email = User.Identity.Name;
            var currentUser = db.GetByEmail(email);
            db.AddAd(currentUser.Id, phonenumber, description);
            return Redirect("/home/index");
        }
       
        [HttpPost]
        public IActionResult DeleteAd(int adId)
        {
            var db = new DbManager(_connectionString);
            db.DeleteAd(adId);
            return RedirectToAction("index");
        }
    }
}
