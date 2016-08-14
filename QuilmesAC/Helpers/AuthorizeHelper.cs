namespace QuilmesAC.Helpers
{
	using System.Web.Mvc;
	using System.Web.Routing;

	public class AuthorizeHelper : AuthorizeAttribute
	{
		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			base.OnAuthorization(filterContext);

			if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
				filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "User", action = "Login" }));

			if (filterContext.Result is HttpUnauthorizedResult)
				filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "User", action = "NotAuthorized" }));
		}
	}
}