using System.Web.Mvc;

namespace WebApplicationWeixin.Areas.area6
{
    public class area6AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "area6";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "area6_default",
                "area6/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}