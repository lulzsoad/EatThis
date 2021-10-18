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
    public interface ICategoryValidator
    {
        void IsNull(Category category);
        Task CheckIfAlreadyExists(Category category);
        Task CheckIfNotFound(int id);
    }

    public class CategoryValidator : ICategoryValidator
    {
        private readonly ICategoryRepository categoryRepository;
        public CategoryValidator(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public void IsNull(Category category)
        {
            if (category == null)
            {
                throw new CustomException(BackendMessage.General.INVALID_OBJECT_MODEL);
            }
        }

        public async Task CheckIfNotFound(int id)
        {
            if (!await categoryRepository.CheckIfExistsById(id))
            {
                throw new CustomException(BackendMessage.Category.CATEGORY_NOT_FOUND);
            }
        }

        public async Task CheckIfAlreadyExists(Category category)
        {
            if (await categoryRepository.CheckIfExists(category))
            {
                throw new CustomException(BackendMessage.Category.CATEGORY_ALREADY_EXISTS);
            }
        }
    }
}
