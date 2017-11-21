using System.Web.Mvc;

namespace WebApplicationWeixin.Areas.area8
{
    public class area8AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "area8";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "area8_default",
                "area8/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}