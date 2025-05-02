using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Verkoop2025Data.Models;

namespace Verkoop2025Web.Filters;

[AttributeUsage(AttributeTargets.Method)]
public class SkipFilterAttribute : Attribute { }


public class LoginActionFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        var hasSkipFilter = context.ActionDescriptor.EndpointMetadata
            .OfType<SkipFilterAttribute>()
            .Any();

        if (hasSkipFilter)
        {
            return;
        }

        string? username = context.HttpContext.Session.GetString("Username");
        if (username == null)
        {
            context.Result = new RedirectToActionResult("Login", "Client", null);
        }
        else
        {
            if (context.Controller is Controller controller)
            {
                controller.ViewBag.CurrentUser = username;
            }
        }
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {

    }
}
