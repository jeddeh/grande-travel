using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;

using WebMatrix.Data;
using WebMatrix.WebData;
using GrandeTravel.Data;

namespace GrandeTravel.Site
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static readonly bool SHOW_SAMPLE_FORM_DATA = true;
        public static readonly int MAX_ACTIVITIES = 3;

        protected void Application_Start()
        {
            System.Data.Entity.Database.SetInitializer<ApplicationDbContext>(null);

            ApplicationDbContext context = new ApplicationDbContext("ApplicationDbContext");
            context.Database.Initialize(true);

            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "Email", true);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}