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
    public interface IUnitValidator
    {
        void IsNull(Unit unit);
        Task CheckIfAlreadyExists(Unit unit);
        Task CheckIfNotFound(int id);
    }

    public class UnitValidator : IUnitValidator
    {
        private readonly IUnitRepository unitRepository;
        public UnitValidator(IUnitRepository unitRepository)
        {
            this.unitRepository = unitRepository;
        }

        public void IsNull(Unit unit)
        {
            if (unit == null)
            {
                throw new CustomException(BackendMessage.General.INVALID_OBJECT_MODEL);
            }
        }

        public async Task CheckIfNotFound(int id)
        {
            if (!await unitRepository.CheckIfExistsById(id))
            {
                throw new CustomException(BackendMessage.Unit.UNIT_NOT_FOUND);
            }
        }

        public async Task CheckIfAlreadyExists(Unit unit)
        {
            if (await unitRepository.CheckIfExists(unit))
            {
                throw new CustomException(BackendMessage.Unit.UNIT_ALREADY_EXISTS);
            }
        }
    }
}
