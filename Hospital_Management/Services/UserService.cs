using Hospital_Management.Models;
using Hospital_Management.Models.DTOS;
using Hospital_Management.Services.Iservice;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hospital_Management.Services
{
    public class UserService:Iuser
    {
        private readonly AppDbContext context;
        private readonly IConfiguration configuration;
        public UserService(AppDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public async Task<string> Register(UserDTO userDTO)
        {
            var user = await context.Users.AnyAsync(x => x.Email == userDTO.Email);
            if (!user)
            {
                var data = new User
                {
                    UserName = userDTO.UserName,
                    Role = userDTO.Role,
                    Email = userDTO.Email,
                    ContactNo = userDTO.ContactNo,
                };
                var hash = new PasswordHasher<User>();
                data.Password = hash.HashPassword(data, userDTO.Password);
                await context.Users.AddAsync(data);
                await context.SaveChangesAsync();
                return "User Registered Successfully";
            }
            return "User Already Exist";

        }

        public async Task<string> Login(LoginDTO loginDTO)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);
            if (user == null)
            {
                return "NotFound";
            }

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.Password, loginDTO.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return "Incorrect";
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId",user.UserId.ToString()),
                new Claim("UserName",user.UserName.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                                                configuration["Jwt:Audience"],
                                                claims, expires: DateTime.UtcNow.AddMinutes(30),
                                                signingCredentials: signIn);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
