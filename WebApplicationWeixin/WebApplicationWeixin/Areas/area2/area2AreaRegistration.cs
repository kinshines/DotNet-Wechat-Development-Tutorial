using System.Web.Mvc;

namespace WebApplicationWeixin.Areas.area2
{
    public class area2AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "area2";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "area2_default",
                "area2/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}