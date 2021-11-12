using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IS.Helpers.Extensions
{
    public static class ClaimExtensions
    {
        public static string GetUserGuid(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
