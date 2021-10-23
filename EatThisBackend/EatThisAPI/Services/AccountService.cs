using AutoMapper;
using EatThisAPI.Models;
using EatThisAPI.Models.DTOs.User;
using EatThisAPI.Repositories;
using EatThisAPI.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EatThisAPI.Services
{
    public interface IAccountService
    {
        Task RegisterUser(RegisterUserDto registerUserDto);
        Task<string> GenerateJwtToken(LoginDto loginDto);
    }

    public class AccountService : IAccountService
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IUserValidator userValidator;
        private readonly AuthenticationSettings authenticationSettings;
        public AccountService(
            IMapper mapper, 
            IUserRepository userRepository, 
            IPasswordHasher<User> passwordHasher,
            IUserValidator userValidator,
            AuthenticationSettings authenticationSettings)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.userValidator = userValidator;
            this.authenticationSettings = authenticationSettings;
        }
        public async Task RegisterUser(RegisterUserDto registerUserDto)
        {
            var newUser = mapper.Map<User>(registerUserDto);
            newUser.PasswordHash = registerUserDto.Password;
            newUser.RegisterDate = DateTime.UtcNow;

            await userValidator.RegisterUserValidate(newUser);

            var hashedPassword = passwordHasher.HashPassword(newUser, registerUserDto.Password);
            newUser.PasswordHash = hashedPassword;

            await userRepository.RegisterUser(newUser);
        }

        public async Task<string> GenerateJwtToken(LoginDto loginDto)
        {
            var user = await userRepository.GetUserByEmail(loginDto.Email);
            userValidator.LoginUserValidate(user);

            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if(verificationResult == PasswordVerificationResult.Failed)
            {
                user = null;
                userValidator.LoginUserValidate(user);
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
                new Claim("RegisterDate", user.RegisterDate.Value.ToString("dd-mm-yyyy"))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey));
            var credentails = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(authenticationSettings.JwtIssuer,
                authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: credentails);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
