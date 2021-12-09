using AutoMapper;
using EatThisAPI.Models.DTOs;
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
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IValidator validator;
        private IUserValidator userValidator;
        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IValidator validator,
            IUserValidator userValidator
            )
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.validator = validator;
            this.userValidator = userValidator;
        }

        public async Task<UserDto> GetUserById(int id)
        {
            validator.IsObjectNull(id);
            var user = await userRepository.GetUserById(id);
            userValidator.IsNull(user);
            return mapper.Map<UserDto>(user);
        }
    }
}
