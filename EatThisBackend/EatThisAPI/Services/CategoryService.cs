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

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, ILogger<CategoryService> logger)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IEnumerable<CategoryDto>> GetAll()
        {
            IEnumerable<Category> categories = await categoryRepository.GetAll();
            IEnumerable<CategoryDto> categoryDtos = mapper.Map<IEnumerable<CategoryDto>>(categories);
            return categoryDtos;
        }

        public async Task<CategoryDto> GetById(int id)
        {
            Category category = await categoryRepository.GetById(id);

            CategoryValidator.Validate(category);
            var categoryDto = mapper.Map<CategoryDto>(category);
            return categoryDto;
        }

        public async Task<int> Add(CategoryDto category)
        {
            if (category == null)
            {
                throw new Exception("Nieprawidłowy model obiektu");
            }

            int id = await categoryRepository.Add(new Category { Id = category.Id, Name = category.Name });
            return id;
        }

        public async Task Delete(CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                throw new Exception("Nieprawidłowy model obiektu");
            }

            await categoryRepository.Delete(new Category { Id = categoryDto.Id, Name = categoryDto.Name });
        }

        public async Task<CategoryDto> Update(CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                throw new Exception("Nieprawidłowy model obiektu");
            }

            Category category = new Category { Id = categoryDto.Id, Name = categoryDto.Name };
            category = await categoryRepository.Update(category);
            categoryDto.Name = category.Name;
            return categoryDto;
        }
    }
}
