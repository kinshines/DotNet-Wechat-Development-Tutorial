using System.Web.Mvc;

namespace WebApplicationWeixin.Areas.area5
{
    public class area5AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "area5";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "area5_default",
                "area5/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}