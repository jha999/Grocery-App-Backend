using APExittest.Business.Repository;
using APExittest.Bussiness.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APExittest.Bussiness.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        //private readonly RoleManager<IdentityUser> _roleManager;

        public AccountRepository(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            //_roleManager = roleManager;
        }
        public async Task<IdentityResult> CreateUserAsync(SignUpModel userModel)
        {

            var user = new ApplicationUser()
            {
                FullName = userModel.FullName,
                Email = userModel.Email,
                UserName = userModel.Email,
                PhoneNumber = userModel.PhoneNumber
            };

            var answer = await _userManager.CreateAsync(user, userModel.Password);
            return answer;
        }

        public async Task<string> PasswordSignInAsync(SignInModel signInModel)
        {
            // rememberme is used for in place of ispersistent similar as cookie to stay sign in the app.

            var answer = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, signInModel.RemberMe, false);
            //return answer;
            if (!answer.Succeeded)
            {
                return null;
            }
            //var userRoles = await _userManager.GetRolesAsync(answer);

            var user = await _userManager.FindByEmailAsync(signInModel.Email);

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email , signInModel.Email),
                //new Claim(ClaimTypes.Name, signInModel.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:ValidIssuer"],
                audience: _configuration["Jwt:ValidAudience"],
                expires: DateTime.UtcNow.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
