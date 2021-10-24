using AutoMapper;
using EatThisAPI.Helpers;
using EatThisAPI.Models;
using EatThisAPI.Models.DTOs.User;
using EatThisAPI.Repositories;
using EatThisAPI.Settings;
using EatThisAPI.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EatThisAPI.Services
{
    public interface IAccountService
    {
        Task RegisterUser(RegisterUserDto registerUserDto);
        Task<string> GenerateJwtToken(LoginDto loginDto);
        Task SendActivatingCode(string email, int userId);
        Task<bool> CheckAndActivateAccount(string activationCode);
    }

    public class AccountService : IAccountService
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IUserValidator userValidator;
        private readonly AuthenticationSettings authenticationSettings;
        private readonly IEmailService emailService;
        private readonly IUserActivatingCodeRepository userActivatingCodeRepository;
        public AccountService(
            IMapper mapper, 
            IUserRepository userRepository, 
            IPasswordHasher<User> passwordHasher,
            IUserValidator userValidator,
            IEmailService emailService,
            IUserActivatingCodeRepository userActivatingCodeRepository,
            AuthenticationSettings authenticationSettings)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.userValidator = userValidator;
            this.authenticationSettings = authenticationSettings;
            this.emailService = emailService;
            this.userActivatingCodeRepository = userActivatingCodeRepository;
        }
        public async Task RegisterUser(RegisterUserDto registerUserDto)
        {
            var newUser = mapper.Map<User>(registerUserDto);
            newUser.PasswordHash = registerUserDto.Password;
            newUser.RegisterDate = DateTime.UtcNow;
            newUser.IsActive = false;

            await userValidator.RegisterUserValidate(newUser);

            var hashedPassword = passwordHasher.HashPassword(newUser, registerUserDto.Password);
            newUser.PasswordHash = hashedPassword;

            int userId = await userRepository.RegisterUser(newUser);
            await SendActivatingCode(newUser.Email, userId);
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

        public async Task SendActivatingCode(string email, int userId)
        {
            StringBuilder sb = new StringBuilder();
            var timestamp = DateTime.UtcNow.ToString();
            foreach(byte b in GetHash(timestamp))
            {
                sb.Append(b.ToString("X2"));
            }

            var activatingCode = sb.ToString();
            var userActivatingCode = new UserActivatingCode
            {
                UserId = userId,
                ActivatingCode = activatingCode
            };

            await userActivatingCodeRepository.Add(userActivatingCode);
            await emailService.SendByNoReply(
                email, 
                "Aktywuj swoje konto", 
                $"{BackendMessage.EmailMessages.EMAILMESSAGE_ACTIVATIONLINK}\n\n{AppSettings.URL_ACCOUNT_ACTIVATION}{activatingCode}");

        }

        public async Task<bool> CheckAndActivateAccount(string activationCode)
        {
            return await userActivatingCodeRepository.CheckAndActivateAccount(activationCode);
        }

        private static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
    }
}
