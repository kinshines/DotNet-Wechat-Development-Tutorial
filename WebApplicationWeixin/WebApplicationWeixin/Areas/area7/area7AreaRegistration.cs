using System.Web.Mvc;

namespace WebApplicationWeixin.Areas.area7
{
    public class area7AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "area7";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "area7_default",
                "area7/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}