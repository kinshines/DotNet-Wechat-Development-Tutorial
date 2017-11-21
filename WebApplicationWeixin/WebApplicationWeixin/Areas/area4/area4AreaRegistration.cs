using System.Web.Mvc;

namespace WebApplicationWeixin.Areas.area4
{
    public class area4AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "area4";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "area4_default",
                "area4/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}