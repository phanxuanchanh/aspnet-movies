using Data.DAL;
using Data.DAOs;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Shared.Mapper;
using Web.Shared.Result;

namespace Data.Services
{
    public class TaxonomyService
    {
        private readonly TaxonomyDao _taxonomyDao;
        private readonly IMapper _mapper;

        public TaxonomyService(TaxonomyDao taxonomyDao, IMapper mapper)
        {
            _taxonomyDao = taxonomyDao;
            _mapper = mapper;
        }

        public async Task<ExecResult<CategoryDto>> GetCategoryAsync(int id)
        {
            Taxonomy taxonomy = await _taxonomyDao.GetAsync(x => x.Id == id);
            if (taxonomy == null)
                return ExecResult<CategoryDto>.NotFound("Category not found.", null);

            CategoryDto categoryDto = _mapper.Map<Taxonomy, CategoryDto>(taxonomy);

            return ExecResult<CategoryDto>
                .Success("Taxonomy retrieved successfully.", categoryDto);
        }

        public async Task<ExecResult<TagDto>> GetTagAsync(int id)
        {
            Taxonomy taxonomy = await _taxonomyDao.GetAsync(x => x.Id == id);
            if (taxonomy == null)
                return new ExecResult<TagDto> { Status = ExecStatus.NotFound, Message = "Tag not found." };

            return new ExecResult<TagDto>
            {
                Status = ExecStatus.Success,
                Message = "People retrieved successfully.",
                Data = new TagDto
                {
                    ID = taxonomy.Id,
                    Name = taxonomy.Name,
                    Description = taxonomy.Description,
                    CreatedAt = taxonomy.CreatedAt,
                    UpdatedAt = taxonomy.UpdatedAt
                }
            };
        }

        public async Task<PagedList<CategoryDto>> GetCategoriesAsync(long pageIndex = 1, long pageSize = 10, string searchText = null)
        {
            long skip = (pageIndex - 1) * pageSize;
            List<Taxonomy> taxonomies = await _taxonomyDao.GetManyAsync("category", skip, pageSize, searchText);

            List<CategoryDto> categories = taxonomies.Select(s => new CategoryDto
            {
                ID = s.Id,
                Name = s.Name,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList();

            long totalItems = await _taxonomyDao.CountAsync("category");

            return new PagedList<CategoryDto>
            {
                Items = categories,
                PageSize = pageSize,
                CurrentPage = pageIndex,
                TotalItems = totalItems
            };
        }

        public async Task<PagedList<TagDto>> GetTagsAsync(long pageIndex = 1, long pageSize = 10, string searchText = null)
        {
            List<Taxonomy> taxonomies = await _taxonomyDao.GetManyAsync("tag", pageIndex, pageSize, searchText);

            List<TagDto> tags = taxonomies.Select(s => new TagDto
            {
                ID = s.Id,
                Name = s.Name,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList();

            long totalItems = await _taxonomyDao.CountAsync("tag");

            return new PagedList<TagDto>
            {
                Items = tags,
                PageSize = pageSize,
                CurrentPage = pageIndex,
                TotalItems = totalItems
            };
        }

        public async Task<ExecResult<CategoryDto>> AddCategoryAsync(CreateCategoryDto input)
        {
            if (string.IsNullOrEmpty(input.Name))
                return new ExecResult<CategoryDto> { Status = ExecStatus.Invalid, Message = "Name is required." };

            Taxonomy taxonomy = new Taxonomy
            {
                Name = input.Name,
                Description = input.Description,
                Type = "category",
                CreatedAt = DateTime.Now,
            };

            int affected = await _taxonomyDao.AddAsync(taxonomy);
            if (affected <= 0)
                return new ExecResult<CategoryDto> { Status = ExecStatus.Failure, Message = "Failed to add category." };

            return new ExecResult<CategoryDto>
            {
                Status = ExecStatus.Success,
                Message = "Category added successfully.",
                Data = new CategoryDto
                {
                    ID = taxonomy.Id,
                    Name = taxonomy.Name,
                    Description = taxonomy.Description,
                    CreatedAt = taxonomy.CreatedAt,
                    UpdatedAt = taxonomy.UpdatedAt
                }
            };
        }

        public async Task<ExecResult<TagDto>> AddTagAsync(CreateTagDto input)
        {
            if (string.IsNullOrEmpty(input.Name))
                return new ExecResult<TagDto> { Status = ExecStatus.Invalid, Message = "Name is required." };

            Taxonomy taxonomy = new Taxonomy
            {
                Name = input.Name,
                Description = input.Description,
                Type = "tag",
                CreatedAt = DateTime.Now,
            };

            int affected = await _taxonomyDao.AddAsync(taxonomy);
            if (affected <= 0)
                return new ExecResult<TagDto> { Status = ExecStatus.Failure, Message = "Failed to add actor." };

            return new ExecResult<TagDto>
            {
                Status = ExecStatus.Success,
                Message = "Actor added successfully.",
                Data = new TagDto
                {
                    ID = taxonomy.Id,
                    Name = taxonomy.Name,
                    Description = taxonomy.Description,
                    CreatedAt = taxonomy.CreatedAt,
                    UpdatedAt = taxonomy.UpdatedAt
                }
            };
        }

        public async Task<ExecResult<CategoryDto>> UpdateCategoryAsync(UpdateCategoryDto input)
        {
            if (string.IsNullOrEmpty(input.Name))
                return new ExecResult<CategoryDto> { Status = ExecStatus.Invalid, Message = "Name is required." };

            Taxonomy taxonomy = new Taxonomy
            {
                Id = input.ID,
                Name = input.Name,
                Description = input.Description,
                Type = "category",
                CreatedAt = DateTime.Now,
            };

            int affected = await _taxonomyDao.UpdateAsync(
                taxonomy, 
                x => x.Id == input.ID, s => new { s.Name, s.Description, s.UpdatedAt });

            if (affected <= 0)
                return new ExecResult<CategoryDto> { Status = ExecStatus.Failure, Message = "Failed to update category." };

            return new ExecResult<CategoryDto>
            {
                Status = ExecStatus.Success,
                Message = "Category updated successfully.",
                Data = new CategoryDto
                {
                    ID = taxonomy.Id,
                    Name = taxonomy.Name,
                    Description = taxonomy.Description,
                    CreatedAt = taxonomy.CreatedAt,
                    UpdatedAt = taxonomy.UpdatedAt
                }
            };
        }

        public async Task<ExecResult<TagDto>> UpdateTagAsync(UpdateTagDto input)
        {
            if (string.IsNullOrEmpty(input.Name))
                return new ExecResult<TagDto> { Status = ExecStatus.Invalid, Message = "Name is required." };

            Taxonomy taxonomy = new Taxonomy
            {
                Id = input.ID,
                Name = input.Name,
                Description = input.Description,
                Type = "tag",
                CreatedAt = DateTime.Now,
            };

            int affected = await _taxonomyDao.UpdateAsync(
                taxonomy,
                x => x.Id == input.ID, s => new { s.Name, s.Description, s.UpdatedAt });

            if (affected <= 0)
                return new ExecResult<TagDto> { Status = ExecStatus.Failure, Message = "Failed to update tag." };

            return new ExecResult<TagDto>
            {
                Status = ExecStatus.Success,
                Message = "Tag updated successfully.",
                Data = new TagDto
                {
                    ID = taxonomy.Id,
                    Name = taxonomy.Name,
                    Description = taxonomy.Description,
                    CreatedAt = taxonomy.CreatedAt,
                    UpdatedAt = taxonomy.UpdatedAt
                }
            };
        }

        public async Task<ExecResult> DeleteAsync(int id, bool forceDelete = false)
        {
            int affected = await _taxonomyDao.DeleteAsync(x => x.Id == id);
            if (affected <= 0)
                return new ExecResult { Status = ExecStatus.NotFound, Message = "Taxonomy not found or deletion failed." };

            return new ExecResult { Status = ExecStatus.Success, Message = "Taxonomy deleted successfully." };
        }
    }
}