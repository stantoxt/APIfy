using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;

namespace APIfy.Example
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
