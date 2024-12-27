using api.Exceptions;
using Microsoft.AspNetCore.Http;
using System;

namespace api.Utils
{
    public class SecurityUtils
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static void VerifyOwnerShip(string resourceOwnerId)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var userId = user.FindFirst("userId")?.Value;
            if (userId != resourceOwnerId)
            {
                throw new UnauthorizedResourceAccessException("User does not own this resource.");
            }
        }
    }
}
