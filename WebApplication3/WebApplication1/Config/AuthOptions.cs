using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Config
{
    public class AuthOptions
    {
        public const string ISSUER = "MyServer";
        public const string AUDIENCE = "MyClient";
        const string KEY = "AndreyBudLikesBeer";
        public const int LIFETIME = 10;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
