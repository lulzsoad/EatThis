using AutoMapper;
using EatThisAPI.Models;
using EatThisAPI.Models.DTOs;
using EatThisAPI.Repositories;
using EatThisAPI.Validators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Services
{
    public interface IUnitService
    {
        Task<IEnumerable<UnitDto>> GetAll();
        Task<UnitDto> GetById(int id);
        Task<int> Add(UnitDto unitDto);
        Task<UnitDto> Update(UnitDto UnitDto);
        Task Delete(UnitDto unitDto);
    }
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository unitRepository;
        private readonly IMapper mapper;
        private readonly ILogger<UnitService> logger;
        private readonly IUnitValidator unitValidator;

        public UnitService(
            IUnitRepository unitRepository,
            IMapper mapper, ILogger<UnitService> logger,
            IUnitValidator unitValidator
            )
        {
            this.unitRepository = unitRepository;
            this.mapper = mapper;
            this.logger = logger;
            this.unitValidator = unitValidator;
        }

        public async Task<IEnumerable<UnitDto>> GetAll()
        {
            IEnumerable<Unit> categories = await unitRepository.GetAll();
            IEnumerable<UnitDto> unitDtos = mapper.Map<IEnumerable<UnitDto>>(categories);
            return unitDtos;
        }

        public async Task<UnitDto> GetById(int id)
        {
            await unitValidator.CheckIfNotFound(id);

            Unit unit = await unitRepository.GetById(id);
            var unitDto = mapper.Map<UnitDto>(unit);
            return unitDto;
        }

        public async Task<int> Add(UnitDto unitDto)
        {
            var unit = mapper.Map<Unit>(unitDto);
            unitValidator.IsNull(unit);
            await unitValidator.CheckIfAlreadyExists(unit);

            int id = await unitRepository.Add(unit);
            return id;
        }

        public async Task Delete(UnitDto unitDto)
        {
            var unit = mapper.Map<Unit>(unitDto);
            unitValidator.IsNull(unit);
            await unitRepository.Delete(unit);
        }

        public async Task<UnitDto> Update(UnitDto unitDto)
        {
            var unit = mapper.Map<Unit>(unitDto);
            unitValidator.IsNull(unit);
            await unitValidator.CheckIfNotFound(unitDto.Id);
            unit = await unitRepository.Update(unit);
            unitDto.Name = unit.Name;
            return unitDto;
        }
    }
}
