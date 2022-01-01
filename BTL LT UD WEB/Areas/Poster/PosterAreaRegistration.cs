using System.Web.Mvc;

namespace BTL_LT_UD_WEB.Areas.Poster
{
    public class PosterAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Poster";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Poster_default",
                "Poster/{controller}/{action}/{id}",
                new { action = "Index", controller = "Poster", id = UrlParameter.Optional },
                new string[] { "BTL_LT_UD_WEB.Areas.Poster.Controllers" }
            );
        }
    }
}