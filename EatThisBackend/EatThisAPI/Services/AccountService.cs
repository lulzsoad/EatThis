using AutoMapper;
using EatThisAPI.Models;
using EatThisAPI.Models.DTOs.User;
using EatThisAPI.Repositories;
using EatThisAPI.Validators;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Services
{
    public interface IAccountService
    {
        Task RegisterUser(RegisterUserDto registerUserDto);
    }

    public class AccountService : IAccountService
    {
        private readonly IMapper mapper;
        private readonly IAccountRepository accountRepository;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IUserValidator userValidator;
        public AccountService(
            IMapper mapper, 
            IAccountRepository accountRepository, 
            IPasswordHasher<User> passwordHasher,
            IUserValidator userValidator)
        {
            this.mapper = mapper;
            this.accountRepository = accountRepository;
            this.passwordHasher = passwordHasher;
            this.userValidator = userValidator;
        }
        public async Task RegisterUser(RegisterUserDto registerUserDto)
        {
            var newUser = mapper.Map<User>(registerUserDto);
            newUser.PasswordHash = registerUserDto.Password;

            await userValidator.RegisterUserValidate(newUser);

            var hashedPassword = passwordHasher.HashPassword(newUser, registerUserDto.Password);
            newUser.PasswordHash = hashedPassword;

            await accountRepository.RegisterUser(newUser);
        }
    }
}
