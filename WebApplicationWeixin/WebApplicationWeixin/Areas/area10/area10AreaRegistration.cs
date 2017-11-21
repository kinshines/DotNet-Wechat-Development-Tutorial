using System.Web.Mvc;

namespace WebApplicationWeixin.Areas.area10
{
    public class area10AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "area10";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "area10_default",
                "area10/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}