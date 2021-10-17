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

        public UnitService(IUnitRepository unitRepository, IMapper mapper, ILogger<UnitService> logger)
        {
            this.unitRepository = unitRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IEnumerable<UnitDto>> GetAll()
        {
            IEnumerable<Unit> units = await unitRepository.GetAll();
            IEnumerable<UnitDto> unitDtos = mapper.Map<IEnumerable<UnitDto>>(units);
            return unitDtos;
        }

        public async Task<UnitDto> GetById(int id)
        {
            Unit unit = await unitRepository.GetById(id);

            UnitValidator.Validate(unit);
            var unitDto = mapper.Map<UnitDto>(unit);
            return unitDto;
        }

        public async Task<int> Add(UnitDto unit)
        {
            if (unit == null)
            {
                throw new Exception("Nieprawidłowy model obiektu");
            }

            int id = await unitRepository.Add(new Unit { Id = unit.Id, Name = unit.Name });
            return id;
        }

        public async Task Delete(UnitDto unitDto)
        {
            if (unitDto == null)
            {
                throw new Exception("Nieprawidłowy model obiektu");
            }

            await unitRepository.Delete(new Unit { Id = unitDto.Id, Name = unitDto.Name });
        }

        public async Task<UnitDto> Update(UnitDto unitDto)
        {
            if (unitDto == null)
            {
                throw new Exception("Nieprawidłowy model obiektu");
            }

            Unit unit = new Unit { Id = unitDto.Id, Name = unitDto.Name };
            unit = await unitRepository.Update(unit);
            unitDto.Name = unit.Name;
            return unitDto;
        }
    }
}
