using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JunkSiteLogin.data;

namespace JunkSiteLogin.web.Models
{
    public class ViewAdsViewModel
    {
        public List<Ad> Ads { get; set; }
        public User CurrentUser { get; set; }
    }
    public class LogInViewModel
    {
        public string Alert { get; set; }
    }
    public class ViewMyAds
    {
        public List<Ad> MyAds { get; set; }
    }
}
