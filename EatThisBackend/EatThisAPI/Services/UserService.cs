using AutoMapper;
using EatThisAPI.Helpers;
using EatThisAPI.Models.DTOs;
using EatThisAPI.Models.DTOs.User;
using EatThisAPI.Repositories;
using EatThisAPI.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Services
{
    public interface IUserService
    {
        Task<UserDto> GetUserById(int id);
        Task<UserDetails> GetCurrentUserDetails();
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IValidator validator;
        private IUserValidator userValidator;
        private readonly IUserHelper userHelper;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IValidator validator,
            IUserValidator userValidator,
            IUserHelper userHelper
            )
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.validator = validator;
            this.userValidator = userValidator;
            this.userHelper = userHelper;
        }

        public async Task<UserDetails> GetCurrentUserDetails()
        {
            var user = await userHelper.GetCurrentUser();

            validator.IsObjectNull(user);

            var userDetails = new UserDetails()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DateOfBirth = user.BirthDate,
                RegisterDate = user.RegisterDate,
                RoleId = user.RoleId,
                Image = user.Image,
                Description = user.Description
            };

            return userDetails;
        }

        public async Task<UserDto> GetUserById(int id)
        {
            validator.IsObjectNull(id);
            var user = await userRepository.GetUserById(id);
            userValidator.UserExists(user);
            return mapper.Map<UserDto>(user);
        }
    }
}
