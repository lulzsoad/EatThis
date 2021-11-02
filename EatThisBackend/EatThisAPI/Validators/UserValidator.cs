using EatThisAPI.Exceptions;
using EatThisAPI.Helpers;
using EatThisAPI.Models;
using EatThisAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Validators
{
    public interface IUserValidator
    {
        Task RegisterUserValidate(User user);
        void IsNull(User user);
        Task ValidateEmail(string email);
        void ValidatePassword(string password);
        void ValidatePersonalData(User user);
        void LoginUserValidate(User user);
        Task EmailExists(string email);
    }
    public class UserValidator : IUserValidator
    {
        private readonly IUserRepository userRepository;
        public UserValidator(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task RegisterUserValidate(User user)
        {
            IsNull(user);
            await ValidateEmail(user.Email);
            ValidatePassword(user.PasswordHash); // Jeszcze nie hashowany
            ValidatePersonalData(user);
        }

        public void LoginUserValidate(User user)
        {
            if(user == null)
            {
                throw new CustomException(BackendMessage.Account.USER_INVALID_EMAIL_OR_PASSWORD);
            }
        }

        public void IsNull(User user)
        {
            if(user == null)
            {
                throw new CustomException(BackendMessage.General.INVALID_OBJECT_MODEL);
            }
        }

        public async Task ValidateEmail(string email)
        {
            if (await userRepository.EmailExists(email))
            {
                throw new CustomException(BackendMessage.Account.USER_EMAIL_ALEADY_TAKEN);
            }
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@') || !email.Contains('.'))
            {
                throw new CustomException(BackendMessage.Account.USER_INVALID_EMAIL);
            }
        }

        public void ValidatePassword(string password)
        {
            if(password.Length < 6 || password == null)
            {
                throw new CustomException(BackendMessage.Account.USER_INVALID_PASSWORD);
            }
        }

        public void ValidatePersonalData(User user)
        {
            if(string.IsNullOrWhiteSpace(user.FirstName))
            {
                throw new CustomException(BackendMessage.Account.USER_FIRST_NAME_REQUIRED);
            }
            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                throw new CustomException(BackendMessage.Account.USER_LAST_NAME_REQUIRED);
            }
        }

        public async Task EmailExists(string email)
        {
            if(!await userRepository.EmailExists(email))
            {
                throw new CustomException(BackendMessage.Account.USER_EMAIL_NOT_FOUND);
            }
        }
    }
}
