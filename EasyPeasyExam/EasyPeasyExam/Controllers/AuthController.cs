
using EasyPeasyExam.Dto;
using EasyPeasyExam.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EasyPeasyExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EasyPeasyExamContext _context;
        private readonly IConfiguration _configuration;
        public AuthController(EasyPeasyExamContext context, IConfiguration configuration)
        {
            this._context = context;
            this._configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            try
            {
                var password = createPasswordHash(registerModel.PasswordHash);
                var dbUser = _context.Users.Where(p => p.Email == registerModel.Email).FirstOrDefault();
                if (dbUser != null)
                {
                    return BadRequest("User is existed");
                }
                var newUser = new User
                {
                    Username = registerModel.Username,
                    Email = registerModel.Email,
                    PasswordHash = password,
                };
                // Thêm role cho người dùng
                string roleName = registerModel.UserRoleName;
                var role = _context.Roles.FirstOrDefault(r => r.RoleName == roleName);
                if (role == null)
                {
                    role = new Role { RoleName = roleName };
                    _context.Roles.Add(role);
                    await _context.SaveChangesAsync();
                }

                var userRole = new UserRole { User = newUser, Role = role };
                _context.UserRoles.Add(userRole);

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return Ok(newUser);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] User user)
        {
            try
            {
                var password = createPasswordHash(user.PasswordHash);
                var dbUser = _context.Users.Include(u => u.UserRoles).Where(u => u.Email == user.Email && password == u.PasswordHash).FirstOrDefault();
                if (dbUser == null)
                {
                    return BadRequest("Username or password is incorrect");
                }
                string roleName = dbUser.UserRoles.Select(r => _context.Roles.FirstOrDefault(role => role.RoleId == r.RoleId)?.RoleName).FirstOrDefault() ?? "No Role";
                List<Claim> authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, dbUser.Email),
                    new Claim("userID", dbUser.UserId.ToString()),
                    new Claim(ClaimTypes.Role, roleName),
                    new Claim("password",user.PasswordHash),
                    new Claim("username",dbUser.Username),
                };
                var token = this.getToken(authClaims);

                var refreshToken = GenerateRefreshToken();
                dbUser.RefreshTokens.Add(refreshToken);
                await _context.SaveChangesAsync();

                SetRefreshToken(refreshToken, dbUser);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        private String createPasswordHash(string password)
        {
            var sha = SHA256.Create();
            var passwordHash = sha.ComputeHash(Encoding.Default.GetBytes(password));
            return Convert.ToBase64String(passwordHash);
        }

        private JwtSecurityToken getToken(List<Claim> authClaim)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: authClaim,
                signingCredentials: cred,
                expires: DateTime.Now.AddDays(1)
                );
            return token;
        }
        private string getNewToken(User user)
        {
            string roleName = user.UserRoles.Select(r => _context.Roles.FirstOrDefault(role => role.RoleId == r.RoleId)?.RoleName).FirstOrDefault() ?? "No Role";

            List<Claim> authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim("userID", user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, roleName),
                };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: authClaims,
                signingCredentials: cred,
                expires: DateTime.Now.AddDays(1)
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiryDate = DateTime.Now.AddDays(7),

            };
            return refreshToken;
        }
        private void SetRefreshToken(RefreshToken newRefreshToken, User user)
        {
            var cookiesOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.ExpiryDate,
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookiesOptions);
            var userFreshToken = user.RefreshTokens.FirstOrDefault(u => u.UserId == user.UserId);
            if (userFreshToken != null)
            {
                userFreshToken.Token = newRefreshToken.Token;
                userFreshToken.ExpiryDate = newRefreshToken.ExpiryDate;
            }

        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshTokenCk = Request.Cookies["refreshToken"];

            var refreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == refreshTokenCk);

            if (refreshToken == null)
            {
                return Unauthorized("Invalid Refresh Token");
            }
            else if (refreshToken.ExpiryDate < DateTime.Now)
            {
                return Unauthorized("token is expired");
            }
            var dbUser = await _context.Users.FindAsync(refreshToken.UserId);


            string token = getNewToken(dbUser);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken, dbUser);

            return Ok(token);
        }

        [HttpPost]
        [Route("login-google")]
        public async Task<IActionResult> GoogleLogin([FromBody] UserGoogleDTO userGoogleDto)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(userGoogleDto.IdToken,
                    new GoogleJsonWebSignature.ValidationSettings());

                // Check if payload is null or not
                if (payload == null)
                {
                    return BadRequest("Invalid Google authentication.");
                }

                // Check if user exists in database
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == payload.Email);

                // If user doesn't exist, create a new one
                if (user == null)
                {
                    user = new User
                    {
                        Username = payload.Name,
                        Email = payload.Email,
                        PasswordHash=null,
                        // Other fields...
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }

                // Generate JWT token and refresh token as usual
                string roleName =user.UserRoles.Select(r => _context.Roles.FirstOrDefault(role => role.RoleId == r.RoleId)?.RoleName).FirstOrDefault() ?? "No Role";
                var authClaims = new List<Claim>
                 {
                    new Claim("username", user.Username.ToString()),
                   new Claim(ClaimTypes.Email, user.Email),
                   new Claim(ClaimTypes.Role, roleName)
            // Other claims...
        };

                var token = this.getToken(authClaims);
                var refreshToken = GenerateRefreshToken();

                user.RefreshTokens.Add(refreshToken);
                await _context.SaveChangesAsync();
                SetRefreshToken(refreshToken, user);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
