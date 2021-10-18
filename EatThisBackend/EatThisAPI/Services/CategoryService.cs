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
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAll();
        Task<CategoryDto> GetById(int id);
        Task<int> Add(CategoryDto categoryDto);
        Task<CategoryDto> Update(CategoryDto CategoryDto);
        Task Delete(CategoryDto categoryDto);
    }
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CategoryService> logger;
        private readonly ICategoryValidator categoryValidator;

        public CategoryService(
            ICategoryRepository categoryRepository, 
            IMapper mapper, ILogger<CategoryService> logger,
            ICategoryValidator categoryValidator
            )
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
            this.logger = logger;
            this.categoryValidator = categoryValidator;
        }

        public async Task<IEnumerable<CategoryDto>> GetAll()
        {
            IEnumerable<Category> categories = await categoryRepository.GetAll();
            IEnumerable<CategoryDto> categoryDtos = mapper.Map<IEnumerable<CategoryDto>>(categories);
            return categoryDtos;
        }

        public async Task<CategoryDto> GetById(int id)
        {
            await categoryValidator.CheckIfNotFound(id);

            Category category = await categoryRepository.GetById(id);
            var categoryDto = mapper.Map<CategoryDto>(category);
            return categoryDto;
        }

        public async Task<int> Add(CategoryDto categoryDto)
        {
            var category = mapper.Map<Category>(categoryDto);
            categoryValidator.IsNull(category);
            await categoryValidator.CheckIfAlreadyExists(category);

            int id = await categoryRepository.Add(category);
            return id;
        }

        public async Task Delete(CategoryDto categoryDto)
        {
            var category = mapper.Map<Category>(categoryDto);
            categoryValidator.IsNull(category);
            await categoryRepository.Delete(category);
        }

        public async Task<CategoryDto> Update(CategoryDto categoryDto)
        {
            var category = mapper.Map<Category>(categoryDto);
            categoryValidator.IsNull(category);
            await categoryValidator.CheckIfNotFound(categoryDto.Id);
            category = await categoryRepository.Update(category);
            categoryDto.Name = category.Name;
            return categoryDto;
        }
    }
}
