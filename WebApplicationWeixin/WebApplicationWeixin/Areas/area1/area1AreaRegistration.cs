using System.Web.Mvc;

namespace WebApplicationWeixin.Areas.area1
{
    public class area1AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "area1";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "area1_default",
                "area1/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}