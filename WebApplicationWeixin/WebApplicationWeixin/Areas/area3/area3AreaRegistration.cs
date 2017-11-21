using System.Web.Mvc;

namespace WebApplicationWeixin.Areas.area3
{
    public class area3AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "area3";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "area3_default",
                "area3/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}