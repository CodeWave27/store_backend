using store_backend.Authorization;

using Microsoft.Extensions.Options;
using store_backend.Helpers;
using store_backend.Dao;

namespace store_backend.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IPersonaDao personaDao, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId= jwtUtils.ValidateJwtToken(token);

            if(userId != null)
            {
                context.Items["Persona"] = personaDao.GetById(userId.Value);
            }
            await _next(context);
        }
    }
}
