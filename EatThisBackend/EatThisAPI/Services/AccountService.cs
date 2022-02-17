using AutoMapper;
using EatThisAPI.Exceptions;
using EatThisAPI.Helpers;
using EatThisAPI.Models;
using EatThisAPI.Models.DTOs.User;
using EatThisAPI.Models.ViewModels;
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
        Task<AuthToken> GenerateJwtToken(LoginDto loginDto);
        Task SendActivatingCode(string email, int userId);
        Task<bool> CheckAndActivateAccount(string activationCode);
        Task<PasswordResetCodeViewModel> GeneratePasswordResetCode(string email);
        Task<PasswordResetCodeViewModel> PasswordResetCodeCheck(PasswordResetCodeViewModel passwordResetCodeVM);
        Task ChangePasswordByResetCode(ChangePasswordResetCodeViewModel changePasswordResetCodeViewModel);
        Task ChangePassword(ChangePasswordViewModel changePasswordVM);
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
        private readonly IPasswordResetCodeRepository passwordResetCodeRepository;
        private readonly IValidator validator;
        private readonly IUserHelper userHelper;
        public AccountService(
            IMapper mapper, 
            IUserRepository userRepository, 
            IPasswordHasher<User> passwordHasher,
            IUserValidator userValidator,
            IEmailService emailService,
            IUserActivatingCodeRepository userActivatingCodeRepository,
            AuthenticationSettings authenticationSettings,
            IPasswordResetCodeRepository passwordResetCodeRepository,
            IValidator validator,
            IUserHelper userHelper)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.userValidator = userValidator;
            this.authenticationSettings = authenticationSettings;
            this.emailService = emailService;
            this.userActivatingCodeRepository = userActivatingCodeRepository;
            this.passwordResetCodeRepository = passwordResetCodeRepository;
            this.validator = validator;
            this.userHelper = userHelper;
        }
        public async Task RegisterUser(RegisterUserDto registerUserDto)
        {
            var newUser = mapper.Map<User>(registerUserDto);
            newUser.PasswordHash = registerUserDto.Password;
            newUser.RegisterDate = DateTime.UtcNow;
            newUser.IsActive = false;
            newUser.RoleId = 3;

            await userValidator.RegisterUserValidate(newUser);

            var hashedPassword = passwordHasher.HashPassword(newUser, registerUserDto.Password);
            newUser.PasswordHash = hashedPassword;

            int userId = await userRepository.RegisterUser(newUser);
            await SendActivatingCode(newUser.Email, userId);
        }

        public async Task ChangePassword(ChangePasswordViewModel changePasswordVM)
        {
            var user = await this.userHelper.GetCurrentUser();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, changePasswordVM.OldPassword);
            if(verificationResult == PasswordVerificationResult.Failed)
            {
                user = null;
                userValidator.LoginUserValidate(user);
            }

            var hashedPassword = passwordHasher.HashPassword(user, changePasswordVM.NewPassword);
            await userRepository.ChangePassword(user.Id, hashedPassword);
        }

        public async Task<AuthToken> GenerateJwtToken(LoginDto loginDto)
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
            var result = tokenHandler.WriteToken(token);
            var authToken = new AuthToken();
            authToken.UserId = user.Id;
            authToken.Email = user.Email;
            authToken.RoleId = user.RoleId;
            authToken.Token = result;
            authToken.TokenExpirationDate = expires;
            return authToken;
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

        public async Task<PasswordResetCodeViewModel> GeneratePasswordResetCode(string email)
        {
            var vm = new PasswordResetCodeViewModel();
            await userValidator.EmailExists(email);
            string code = GenerateRandomString();

            await passwordResetCodeRepository.AddResetCode(email, code);
            await emailService.SendByNoReply(email, BackendMessage.EmailMessages.EMAILMESSAGE_RESET_YOUR_PASSWORD, $"{BackendMessage.EmailMessages.EMAILMESSAGE_RESET_YOUR_PASSWORD_MESSAGE}\n\n{code}");
            
            vm.Email = email;
            return vm;
        }

        public async Task<PasswordResetCodeViewModel> PasswordResetCodeCheck(PasswordResetCodeViewModel passwordResetCodeVM)
        {
            var vm = new PasswordResetCodeViewModel();
            await userValidator.EmailExists(passwordResetCodeVM.Email);

            StringBuilder sb = new StringBuilder();
            var timestamp = DateTime.UtcNow.ToString();
            foreach (byte b in GetHash(timestamp))
            {
                sb.Append(b.ToString("X2"));
            }
            var securedRoute = sb.ToString();

            var prc = new PasswordResetCode { Email = passwordResetCodeVM.Email, Code = passwordResetCodeVM.Code, SecuredRoute = securedRoute };
            prc = await passwordResetCodeRepository.CheckPasswordResetCode(prc);
            userValidator.IsPasswordCodeCorrect(prc);

            vm.Code = prc.Code;
            vm.Email = prc.Email;
            vm.SecuredRoute = prc.SecuredRoute;
            return vm;
        }

        public async Task ChangePasswordByResetCode(ChangePasswordResetCodeViewModel changePasswordResetCodeViewModel)
        {
            validator.IsObjectNull(changePasswordResetCodeViewModel);
            userValidator.ValidatePassword(changePasswordResetCodeViewModel.Password);
            var prc = await passwordResetCodeRepository
                .GetPasswordResetCodeBySecuredRouteAndEmail(
                    changePasswordResetCodeViewModel.PasswordResetCode.SecuredRoute,
                    changePasswordResetCodeViewModel.PasswordResetCode.Email
                    );

            if(prc == null || prc.Code != null)
            {
                throw new CustomException(BackendMessage.Account.PASSWORD_RESET_CODE_ERROR);
            }

            var user = await userRepository.GetUserByEmail(changePasswordResetCodeViewModel.PasswordResetCode.Email);
            validator.IsObjectNull(user);
            var hashedPassword = passwordHasher.HashPassword(user, changePasswordResetCodeViewModel.Password);
            user.PasswordHash = hashedPassword;

            await passwordResetCodeRepository.ChangePasswordByResetCode(prc, user);
        }

        private static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        private static string GenerateRandomString()
        {
            Random rand = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[rand.Next(s.Length)]).ToArray());
        }
    }
}
