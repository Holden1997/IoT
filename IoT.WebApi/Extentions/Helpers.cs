using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace IoT.WebApi.Extentions
{
    public static class Helpers
    {
        public static Guid ClientId(this HttpContext context)
        {
            var claimsIdentity = context.User.Identities.ToList();
            var claimsUser = claimsIdentity[0].Claims.ToList();

            return Guid.Parse(claimsUser[2].Value);
        }

    }
}
