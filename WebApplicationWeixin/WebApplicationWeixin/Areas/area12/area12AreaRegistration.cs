using System.Web.Mvc;

namespace WebApplicationWeixin.Areas.area12
{
    public class area12AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "area12";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "area12_default",
                "area12/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}