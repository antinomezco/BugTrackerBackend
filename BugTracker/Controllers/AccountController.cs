using BugTracker.DTOs.Person;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BugTracker.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(
            UserManager<IdentityUser> userManager, 
            IConfiguration configuration, 
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        [HttpGet("RenewToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<AuthenticationResponse>> Renew()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var userCredentials = new UserCredentials()
            {
                Email = email,
            };
            return await BuildToken(userCredentials);
        }

        [HttpPost("MakeAdmin")]
        public async Task<ActionResult> MakeAdmin(AddRemoveClaim addRemoveClaim)
        {
            var user = await _userManager.FindByEmailAsync(addRemoveClaim.Email);
            await _userManager.AddClaimAsync(user, new Claim("IsAdmin", "1"));
            return NoContent();
        }

        [HttpPost("RemoveAdmin")]
        public async Task<ActionResult> RemoveAdmin(AddRemoveClaim addRemoveClaim)
        {
            var user = await _userManager.FindByEmailAsync(addRemoveClaim.Email);
            await _userManager.RemoveClaimAsync(user, new Claim("IsAdmin", "1"));
            return NoContent();
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationResponse>> Register(UserCredentials userCredentials)
        {
            var user = new IdentityUser { UserName = userCredentials.Email, Email = userCredentials.Email };
            var result = await _userManager.CreateAsync(user, userCredentials.Password);

            if (result.Succeeded)
                return await BuildToken(userCredentials);
            else
                return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(UserCredentials userCredentials)
        {
            var result = await _signInManager.PasswordSignInAsync(userCredentials.Email, userCredentials.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
                return await BuildToken(userCredentials);
            else
                return BadRequest("Incorrect user details");
        }

        private async Task<AuthenticationResponse> BuildToken(UserCredentials userCredentials)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", userCredentials.Email),
                new Claim("whatever", "value")
            };

            var user = await _userManager.FindByEmailAsync(userCredentials.Email);
            var claimsDb = await _userManager.GetClaimsAsync(user);

            claims.AddRange(claimsDb);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtkey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddYears(1);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);

            return new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiration = expiration
            };
        }
    }
}
