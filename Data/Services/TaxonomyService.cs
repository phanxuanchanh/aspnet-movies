using Common;
using Common.Web;
using Data.BLL;
using Data.DAL;
using Data.DTO;
using MSSQL.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Services
{
    public class TaxonomyService : IDisposable
    {
        private readonly GeneralDao _generalDao;
        private readonly TaxonomyDao _taxonomyDao;
        private bool disposedValue;

        public TaxonomyService(GeneralDao generalDao)
        {
            _generalDao = generalDao;
            _taxonomyDao = _generalDao.TaxonomyDao;
        }

        public async Task<ExecResult<CategoryDto>> GetCategoryAsync(int id)
        {
            Taxonomy taxonomy = await _taxonomyDao.GetAsync(id);
            if (taxonomy == null)
                return new ExecResult<CategoryDto> { Status = ExecStatus.NotFound, Message = "Category not found." };

            return new ExecResult<CategoryDto>
            {
                Status = ExecStatus.Success,
                Message = "Taxonomy retrieved successfully.",
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

        public async Task<ExecResult<TagDto>> GetTagAsync(int id)
        {
            Taxonomy taxonomy = await _taxonomyDao.GetAsync(id);
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

        public async Task<PagedList<CategoryDto>> GetCategoriesAsync(long pageIndex = 1, long pageSize = 10)
        {
            PagedList<Taxonomy> data = await _taxonomyDao.GetsAsync("category", pageIndex, pageSize);

            List<CategoryDto> categories = data.Items.Select(s => new CategoryDto
            {
                ID = s.Id,
                Name = s.Name,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList();

            return new PagedList<CategoryDto>
            {
                Items = categories,
                PageNumber = data.PageNumber,
                CurrentPage = data.CurrentPage,
            };
        }

        public async Task<PagedList<TagDto>> GetTagsAsync(long pageIndex = 1, long pageSize = 10)
        {
            PagedList<Taxonomy> data = await _taxonomyDao.GetsAsync("tag", pageIndex, pageSize);

            List<TagDto> Tags = data.Items.Select(s => new TagDto
            {
                ID = s.Id,
                Name = s.Name,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList();

            return new PagedList<TagDto>
            {
                Items = Tags,
                PageNumber = data.PageNumber,
                CurrentPage = data.CurrentPage,
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

            int affected = await _taxonomyDao.UpdateAsync(taxonomy);
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

            int affected = await _taxonomyDao.UpdateAsync(taxonomy);
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
            int affected = await _taxonomyDao.DeleteAsync(id, forceDelete);
            if (affected <= 0)
                return new ExecResult { Status = ExecStatus.NotFound, Message = "Taxonomy not found or deletion failed." };

            return new ExecResult { Status = ExecStatus.Success, Message = "Taxonomy deleted successfully." };
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                _generalDao.Dispose();
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}