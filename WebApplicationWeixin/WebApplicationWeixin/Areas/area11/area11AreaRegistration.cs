using System.Web.Mvc;

namespace WebApplicationWeixin.Areas.area11
{
    public class area11AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "area11";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "area11_default",
                "area11/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}