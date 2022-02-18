using EatThisAPI.Models.DTOs;
using EatThisAPI.Repositories;
using EatThisAPI.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Services
{
    public interface IRoleService
    {
        Task<RoleDto> GetRoleById(int id);
        Task<List<RoleDto>> GetRoles();
    }
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository roleRepository;
        private readonly IValidator validator;
        public RoleService(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }
        public async Task<RoleDto> GetRoleById(int id)
        {
            validator.IsObjectNull(id);
            var roleDto = new RoleDto();
            var role = await roleRepository.GetRoleById(id);
            roleDto.Id = role.Id;
            roleDto.Name = role.Name;
            return roleDto;
        }

        public async Task<List<RoleDto>> GetRoles()
        {
            var roles = await roleRepository.GetRoles();
            var rolesDto = new List<RoleDto>();
            foreach(var role in roles)
            {
                rolesDto.Add(new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name
                });
            }

            return rolesDto;
        }
    }
}
