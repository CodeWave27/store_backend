using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using store_backend.Dto;
using store_backend.Enums;

namespace store_backend.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute: Attribute, IAuthorizationFilter
    {
        private readonly IList<Roles> _roles;

        public AuthorizeAttribute(params Roles[] roles)
        {
            _roles = roles ?? new Roles[] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
            {
                return;
            }

            //Authorization
            var user = (PersonaDTO)context.HttpContext.Items["Persona"];
     
            if (user == null || (_roles.Any() && !_roles.Contains(user.Role)))
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
