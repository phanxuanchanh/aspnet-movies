using Data.DAOs;
using Data.DTO;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Shared.Result;

namespace Data.Services
{
    public class FilmMetadataService
    {
        private readonly FilmMetadataDao _filmMetadataDao;

        public FilmMetadataService(FilmMetadataDao metadataDao) { 
            _filmMetadataDao = metadataDao;
        }

        public async Task<ExecResult<CountryDto>> GetCountryAsync(int id)
        {
            FilmMetadata metadata = await _filmMetadataDao.GetAsync(x => x.Id == id);
            if (metadata == null)
                return new ExecResult<CountryDto> { Status = ExecStatus.NotFound, Message = "Metadata not found." };

            return new ExecResult<CountryDto>
            {
                Status = ExecStatus.Success,
                Message = "Metadata retrieved successfully.",
                Data = new CountryDto
                {
                    ID = metadata.Id,
                    Name = metadata.Name,
                    Description = metadata.Description,
                    CreatedAt = metadata.CreatedAt,
                    UpdatedAt = metadata.UpdatedAt
                }
            };
        }

        public async Task<ExecResult<LanguageDto>> GetLanguageAsync(int id)
        {
            FilmMetadata metadata = await _filmMetadataDao.GetAsync(x => x.Id == id);
            if (metadata == null)
                return new ExecResult<LanguageDto> { Status = ExecStatus.NotFound, Message = "Metadata not found." };

            return new ExecResult<LanguageDto>
            {
                Status = ExecStatus.Success,
                Message = "Metadata retrieved successfully.",
                Data = new LanguageDto
                {
                    ID = metadata.Id,
                    Name = metadata.Name,
                    Description = metadata.Description,
                    CreatedAt = metadata.CreatedAt,
                    UpdatedAt = metadata.UpdatedAt
                }
            };
        }

        public async Task<PagedList<CountryDto>> GetCountriesAsync(long pageIndex = 1, long pageSize = 10, string searchText = null)
        {
            long skip = (pageIndex - 1) * pageSize;
            List<FilmMetadata> metadata =  await _filmMetadataDao.GetsAsync("country", skip, take: pageSize, searchText);

            List<CountryDto> countries = metadata.Select(s => new CountryDto
            {
                ID = s.Id,
                Name = s.Name,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList();

            long totalItems = await _filmMetadataDao.CountAsync("country", searchText);

            return new PagedList<CountryDto>
            {
                Items = countries,
                PageSize = pageSize,
                CurrentPage = pageIndex,
                TotalItems = totalItems
            };
        }

        public async Task<PagedList<LanguageDto>> GetLanguagesAsync(long pageIndex = 1, long pageSize = 10, string searchText = null)
        {
            long skip = (pageIndex - 1) * pageSize;
            List<FilmMetadata> metadata = await _filmMetadataDao.GetsAsync("language", pageIndex, pageSize, searchText);
            
            List<LanguageDto> languages = metadata.Select(s => new LanguageDto
            {
                ID = s.Id,
                Name = s.Name,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList();

            long totalItems = await _filmMetadataDao.CountAsync("language", searchText);

            return new PagedList<LanguageDto>
            {
                Items = languages,
                PageSize = pageSize,
                CurrentPage = pageIndex,
                TotalItems = totalItems
            };
        }

        public async Task<ExecResult<CountryDto>> AddCountryAsync(CreateCountryDto input)
        {
            if(string.IsNullOrEmpty(input.Name))
                return new ExecResult<CountryDto> { Status = ExecStatus.Invalid, Message = "Name is required." };

            FilmMetadata filmMetadata = new FilmMetadata
            {
                Name = input.Name,
                Description = input.Description,
                Type = "country",
                CreatedAt = DateTime.Now,
            };

            int affected = await _filmMetadataDao.AddAsync(filmMetadata);
            if (affected <= 0)
                return new ExecResult<CountryDto> { Status = ExecStatus.Failure, Message = "Failed to add country." };

            return new ExecResult<CountryDto> { 
                Status = ExecStatus.Success,
                Message = "Country added successfully.",
                Data = new CountryDto
                {
                    ID = filmMetadata.Id,
                    Name = filmMetadata.Name,
                    Description = filmMetadata.Description,
                    CreatedAt = filmMetadata.CreatedAt,
                    UpdatedAt = filmMetadata.UpdatedAt
                }
            };
        }

        public async Task<ExecResult<LanguageDto>> AddLanguageAsync(CreateLanguageDto input)
        {
            if (string.IsNullOrEmpty(input.Name))
                return new ExecResult<LanguageDto> { Status = ExecStatus.Invalid, Message = "Name is required." };

            FilmMetadata filmMetadata = new FilmMetadata
            {
                Name = input.Name,
                Description = input.Description,
                Type = "language",
                CreatedAt = DateTime.Now,
            };

            int affected = await _filmMetadataDao.AddAsync(filmMetadata);
            if (affected <= 0)
                return new ExecResult<LanguageDto> { Status = ExecStatus.Failure, Message = "Failed to add language." };

            return new ExecResult<LanguageDto>
            {
                Status = ExecStatus.Success,
                Message = "Country added successfully.",
                Data = new LanguageDto
                {
                    ID = filmMetadata.Id,
                    Name = filmMetadata.Name,
                    Description = filmMetadata.Description,
                    CreatedAt = filmMetadata.CreatedAt,
                    UpdatedAt = filmMetadata.UpdatedAt
                }
            };
        }

        public async Task<ExecResult<CountryDto>> UpdateCountryAsync(UpdateCountryDto input)
        {
            if (string.IsNullOrEmpty(input.Name))
                return new ExecResult<CountryDto> { Status = ExecStatus.Invalid, Message = "Name is required." };

            FilmMetadata filmMetadata = new FilmMetadata
            {
                Id = input.ID,
                Name = input.Name,
                Description = input.Description,
                Type = "country",
                CreatedAt = DateTime.Now,
            };

            int affected = await _filmMetadataDao.UpdateAsync(
                filmMetadata,
                x => x.Id == input.ID, s => new { s.Name, s.Description, s.UpdatedAt });
            if (affected <= 0)
                return new ExecResult<CountryDto> { Status = ExecStatus.Failure, Message = "Failed to update country." };

            return new ExecResult<CountryDto>
            {
                Status = ExecStatus.Success,
                Message = "Country updated successfully.",
                Data = new CountryDto
                {
                    ID = filmMetadata.Id,
                    Name = filmMetadata.Name,
                    Description = filmMetadata.Description,
                    CreatedAt = filmMetadata.CreatedAt,
                    UpdatedAt = filmMetadata.UpdatedAt
                }
            };
        }

        public async Task<ExecResult<LanguageDto>> UpdateLanguageAsync(UpdateLanguageDto input)
        {
            if (string.IsNullOrEmpty(input.Name))
                return new ExecResult<LanguageDto> { Status = ExecStatus.Invalid, Message = "Name is required." };

            FilmMetadata filmMetadata = new FilmMetadata
            {
                Id = input.ID,
                Name = input.Name,
                Description = input.Description,
                Type = "language",
                CreatedAt = DateTime.Now,
            };

            int affected = await _filmMetadataDao.UpdateAsync(
                filmMetadata,
                x => x.Id == input.ID, s => new { s.Name, s.Description, s.UpdatedAt });
            if (affected <= 0)
                return new ExecResult<LanguageDto> { Status = ExecStatus.Failure, Message = "Failed to update country." };

            return new ExecResult<LanguageDto>
            {
                Status = ExecStatus.Success,
                Message = "Country updated successfully.",
                Data = new LanguageDto
                {
                    ID = filmMetadata.Id,
                    Name = filmMetadata.Name,
                    Description = filmMetadata.Description,
                    CreatedAt = filmMetadata.CreatedAt,
                    UpdatedAt = filmMetadata.UpdatedAt
                }
            };
        }

        public async Task<ExecResult> DeleteAsync(int id, bool forceDelete = false)
        {
            int affected = await _filmMetadataDao.DeleteAsync(x => x.Id == id);
            if (affected <= 0)
                return new ExecResult { Status = ExecStatus.NotFound, Message = "Country not found or deletion failed." };
            
            return new ExecResult { Status = ExecStatus.Success, Message = "Country deleted successfully." };
        }
    }
}
