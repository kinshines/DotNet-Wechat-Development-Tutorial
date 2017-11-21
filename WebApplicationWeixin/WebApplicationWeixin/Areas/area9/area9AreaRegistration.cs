using System.Web.Mvc;

namespace WebApplicationWeixin.Areas.area9
{
    public class area9AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "area9";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "area9_default",
                "area9/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}