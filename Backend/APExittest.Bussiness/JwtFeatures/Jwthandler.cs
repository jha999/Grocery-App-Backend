//using APExittest.Bussiness.Model;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;

//namespace APExittest.Bussiness.JwtFeatures
//{
//    public class Jwthandler
//    {
//        private readonly IConfiguration _configuration;
//        private readonly UserManager<ApplicationUser> _userManager;

//        public Jwthandler(IConfiguration configuration, UserManager<ApplicationUser> userManager)
//        {
//            _userManager = userManager;
//            _configuration = configuration;
//        }

//        public SigningCredentials GetSigningCredentials()
//        {
//            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("securityKey").Value);
//            var secret = new SymmetricSecurityKey(key);
//            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
//        }

//        public async Task<List<Claim>> GetClaimsAsync(SignInModel signInModel)
//        {
//            var user = await _userManager.FindByEmailAsync(signInModel.Email);

//            var userRoles = await _userManager.GetRolesAsync(user);

//            var authClaims = new List<Claim>
//            {
//            new Claim(ClaimTypes.Name, signInModel.Email)
//            };

//            foreach (var userRole in userRoles)
//            {
//                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
//            }


//            return authClaims;
//        }

//        public JwtSecurityToken GenerateTokenOptions(List<Claim> claims)
//        {
//            var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]));

//            var tokenOptions = new JwtSecurityToken(
//                issuer: _configuration["Jwt:ValidIssuer"],
//                audience: _configuration["Jwt:ValidAudience"],
//                claims: claims,
//                expires: DateTime.UtcNow.AddMinutes(30),
//                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
//                );
//            return tokenOptions;
//        }
//    }
//}
