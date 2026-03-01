using GeorgeShop.DAL.DTO.Request;
using GeorgeShop.DAL.DTO.Response;
using GeorgeShop.DAL.Models;
using GeorgeShop.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.BLL.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponse> CreateCategory(CategoryRequest request, CancellationToken cancellationToken)
        {
            var category = request.Adapt<Category>();
            await Task.Delay(5000, cancellationToken);
            await _categoryRepository.CreateAsync(category , cancellationToken);

            return category.Adapt<CategoryResponse>();
        }

        public async Task<List<CategoryResponse>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync(new string[]
            {
                nameof(Category.Translations)
            });
            return categories.Adapt<List<CategoryResponse>>();
        }

        public async Task<CategoryResponse?> GetCategory(Expression<Func<Category,bool>> filter)
        {
            var category = await _categoryRepository.GetOne(filter , new string[]
            {
                nameof(Category.Translations)
            });

            return category.Adapt<CategoryResponse>();
        }
    }
}
