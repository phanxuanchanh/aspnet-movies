using Data.DAOs;
using Data.DTO;
using Data.Models;
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
        private readonly TaxonomyLinkDao _taxonomyLinkDao;
        private readonly IMapper _mapper;

        public TaxonomyService(TaxonomyDao taxonomyDao, TaxonomyLinkDao taxonomyLinkDao, IMapper mapper)
        {
            _taxonomyDao = taxonomyDao;
            _taxonomyLinkDao = taxonomyLinkDao;
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
                return ExecResult<TagDto>.NotFound("Tag not found.", null);

            TagDto tagDto = _mapper.Map<Taxonomy, TagDto>(taxonomy);
            return ExecResult<TagDto>.Success("People retrieved successfully.", tagDto);
        }

        public async Task<PagedList<CategoryDto>> GetCategoriesAsync(long pageIndex = 1, long pageSize = 10, string searchText = null)
        {
            long skip = (pageIndex - 1) * pageSize;
            List<Taxonomy> taxonomies = await _taxonomyDao.GetManyAsync("category", skip, pageSize, searchText);

            List<CategoryDto> categories = taxonomies
                .Select(s => _mapper.Map<Taxonomy, CategoryDto>(s)).ToList();

            long totalItems = await _taxonomyDao.CountAsync("category");

            return new PagedList<CategoryDto>
            {
                Items = categories,
                PageSize = pageSize,
                CurrentPage = pageIndex,
                TotalItems = totalItems
            };
        }

        public async Task<List<CategoryDto>> GetCategoriesByFilmIdAsync(string filmId)
        {
            List<TaxonomyLink> taxonomyLinks = await _taxonomyLinkDao
                .GetManyByFilmIdAsync(filmId);

            if (taxonomyLinks == null || taxonomyLinks.Count == 0)
                return new List<CategoryDto>();

            int[] taxonomyIds = taxonomyLinks.Select(s => s.TaxonomyId).ToArray();
            List<Taxonomy> taxonomies = await _taxonomyDao.GetsByIdsAsync(taxonomyIds);

            return taxonomies.Select(s => _mapper.Map<Taxonomy, CategoryDto>(s)).ToList();
        }

        public async Task<PagedList<TagDto>> GetTagsAsync(long pageIndex = 1, long pageSize = 10, string searchText = null)
        {
            List<Taxonomy> taxonomies = await _taxonomyDao.GetManyAsync("tag", pageIndex, pageSize, searchText);
            
            List<TagDto> tags = taxonomies
                .Select(s => _mapper.Map<Taxonomy, TagDto>(s)).ToList();

            long totalItems = await _taxonomyDao.CountAsync("tag");

            return new PagedList<TagDto>
            {
                Items = tags,
                PageSize = pageSize,
                CurrentPage = pageIndex,
                TotalItems = totalItems
            };
        }

        public async Task<List<TagDto>> GetTagsByFilmIdAsync(string filmId)
        {
            List<TaxonomyLink> taxonomyLinks = await _taxonomyLinkDao
                .GetManyByFilmIdAsync(filmId);

            if (taxonomyLinks == null || taxonomyLinks.Count == 0)
                return new List<TagDto>();

            int[] taxonomyIds = taxonomyLinks.Select(s => s.TaxonomyId).ToArray();
            List<Taxonomy> taxonomies = await _taxonomyDao.GetsByIdsAsync(taxonomyIds);

            return taxonomies.Select(s => _mapper.Map<Taxonomy, TagDto>(s)).ToList();
        }

        public async Task<ExecResult<CategoryDto>> AddCategoryAsync(CreateCategoryDto input)
        {
            Taxonomy taxonomy = new Taxonomy
            {
                Name = input.Name,
                Description = input.Description,
                Type = "category",
                CreatedAt = DateTime.Now,
            };

            int affected = await _taxonomyDao.AddAsync(taxonomy);
            if (affected <= 0)
                return ExecResult<CategoryDto>.Failure("Failed to add category.", null);

            CategoryDto categoryDto = _mapper.Map<Taxonomy, CategoryDto>(taxonomy);
            return ExecResult<CategoryDto>.Success("Category added successfully.", categoryDto);
        }

        public async Task<ExecResult<TagDto>> AddTagAsync(CreateTagDto input)
        {
            Taxonomy taxonomy = new Taxonomy
            {
                Name = input.Name,
                Description = input.Description,
                Type = "tag",
                CreatedAt = DateTime.Now,
            };

            int affected = await _taxonomyDao.AddAsync(taxonomy);
            if (affected <= 0)
                return ExecResult<TagDto>.Failure("Failed to add actor.", null);

            TagDto tagDto = _mapper.Map<Taxonomy, TagDto>(taxonomy);
            return ExecResult<TagDto>.Success("Actor added successfully.", tagDto);
        }

        public async Task<ExecResult<CategoryDto>> UpdateCategoryAsync(UpdateCategoryDto input)
        {
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
                return ExecResult<CategoryDto>.Failure("Failed to update category.", null);

            CategoryDto categoryDto = _mapper.Map<Taxonomy, CategoryDto>(taxonomy);
            return ExecResult<CategoryDto>.Success("Category updated successfully.", categoryDto);
        }

        public async Task<ExecResult<TagDto>> UpdateTagAsync(UpdateTagDto input)
        {
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
                return ExecResult<TagDto>.Failure("Failed to update tag.", null);

            TagDto tagDto = _mapper.Map<Taxonomy, TagDto>(taxonomy);
            return ExecResult<TagDto>.Success("Tag updated successfully.", tagDto);
        }

        public async Task<ExecResult> DeleteAsync(int id, bool forceDelete = false)
        {
            Taxonomy taxonomy = await _taxonomyDao.GetAsync(x => x.Id == id);
            if (taxonomy == null)
                return ExecResult.NotFound("Taxonomy not found.");

            if (forceDelete)
            {
                int affected = await _taxonomyDao.DeleteAsync(x => x.Id == id);
                if (affected <= 0)
                    return ExecResult.Failure("Taxonomy not found or deletion failed.");

                return ExecResult.Success("Taxonomy deleted successfully.");
            }

            taxonomy.DeletedAt = DateTime.Now;
            int updated = await _taxonomyDao.UpdateAsync(taxonomy, x => x.Id == id, s => new { s.DeletedAt });
            if (updated <= 0)
                return ExecResult.Failure("Failed to move taxonomy to trash.");

            return ExecResult.Success("Taxonomy moved to trash.");
        }
    }
}