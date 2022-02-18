using AutoMapper;
using EatThisAPI.Exceptions;
using EatThisAPI.Helpers;
using EatThisAPI.Models;
using EatThisAPI.Models.DTOs;
using EatThisAPI.Models.DTOs.User;
using EatThisAPI.Models.ViewModels;
using EatThisAPI.Repositories;
using EatThisAPI.Validators;
using Microsoft.AspNetCore.JsonPatch;
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
        Task<UserDetails> UpdateCurrentUser(JsonPatchDocument userDetails);
        Task<DataChunkViewModel<UserDetails>> GetChunkOfUsers(int skip, int take, string search);
        Task ChangeUserRole(int userId, RoleDto roleDto);
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

        public async Task<UserDetails> UpdateCurrentUser(JsonPatchDocument userDetailsDocument)
        {
            var user = await userHelper.GetCurrentUser();
            validator.IsObjectNull(userDetailsDocument);
            userValidator.UserExists(user);
            user = await userRepository.UpdateUser(user, userDetailsDocument);
            var userDetails = mapper.Map<UserDetails>(user);
            return userDetails;
        }

        public async Task<DataChunkViewModel<UserDetails>> GetChunkOfUsers(int skip, int take, string search)
        {
            var users = await userRepository.GetChunkOfUsers(skip, take, search);
            validator.IsObjectNull(users);

            var usersDto = new DataChunkViewModel<UserDetails>();
            usersDto.Data = new List<UserDetails>();
            usersDto.Total = users.Total;
            foreach(var user in users.Data)
            {
                usersDto.Data.Add(new UserDetails
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = new RoleDto
                    {
                        Id = user.Role.Id,
                        Name = user.Role.Name
                    },
                    IsActive = user.IsActive
                });
            }

            return usersDto;
        }

        public async Task ChangeUserRole(int userId, RoleDto roleDto)
        {
            await userRepository.ChangeUserRole(userId, roleDto.Id);
        }
    }
}
