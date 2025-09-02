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
    public class FilmMetadataService
    {
        private readonly FilmMetadataDao _filmMetadataDao;
        private readonly IMapper _mapper;

        public FilmMetadataService(FilmMetadataDao metadataDao, IMapper mapper) { 
            _filmMetadataDao = metadataDao;
            _mapper = mapper;
        }

        public async Task<ExecResult<CountryDto>> GetCountryAsync(int id)
        {
            FilmMetadata metadata = await _filmMetadataDao.GetAsync(x => x.Id == id);
            if (metadata == null)
                return ExecResult<CountryDto>.NotFound("Country not found.", null);

            CountryDto countryDto = _mapper.Map<FilmMetadata, CountryDto>(metadata);
            return ExecResult<CountryDto>.Success("Metadata retrieved successfully.", countryDto);
        }

        public async Task<ExecResult<LanguageDto>> GetLanguageAsync(int id)
        {
            FilmMetadata metadata = await _filmMetadataDao.GetAsync(x => x.Id == id);
            if (metadata == null)
                return ExecResult<LanguageDto>.NotFound("Language not found.", null);

            LanguageDto languageDto = _mapper.Map<FilmMetadata, LanguageDto>(metadata);
            return ExecResult<LanguageDto>.Success("Metadata retrieved successfully.", languageDto);
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
            
            List<LanguageDto> languages = metadata
                .Select(s => _mapper.Map<FilmMetadata, LanguageDto>(s)).ToList();

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
            FilmMetadata filmMetadata = new FilmMetadata
            {
                Name = input.Name,
                Description = input.Description,
                Type = "country",
                CreatedAt = DateTime.Now,
            };

            int affected = await _filmMetadataDao.AddAsync(filmMetadata);
            if (affected <= 0)
                return ExecResult<CountryDto>.Failure("Failed to add country.", null);

            CountryDto countryDto = _mapper.Map<FilmMetadata, CountryDto>(filmMetadata);
            return ExecResult<CountryDto>.Success("Country added successfully.", countryDto);
        }

        public async Task<ExecResult<LanguageDto>> AddLanguageAsync(CreateLanguageDto input)
        {
            FilmMetadata filmMetadata = new FilmMetadata
            {
                Name = input.Name,
                Description = input.Description,
                Type = "language",
                CreatedAt = DateTime.Now,
            };

            int affected = await _filmMetadataDao.AddAsync(filmMetadata);
            if (affected <= 0)
                return ExecResult<LanguageDto>.Failure("Failed to add language.", null);

            LanguageDto languageDto = _mapper.Map<FilmMetadata, LanguageDto>(filmMetadata);
            return ExecResult<LanguageDto>.Success("Language added successfully.", languageDto);
        }

        public async Task<ExecResult<CountryDto>> UpdateCountryAsync(UpdateCountryDto input)
        {
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
                return ExecResult<CountryDto>.Failure("Failed to update country.", null);

            CountryDto countryDto = _mapper.Map<FilmMetadata, CountryDto>(filmMetadata);
            return ExecResult<CountryDto>.Success("Country updated successfully.", countryDto);
        }

        public async Task<ExecResult<LanguageDto>> UpdateLanguageAsync(UpdateLanguageDto input)
        {
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
                return ExecResult<LanguageDto>.Failure("Failed to update country.", null);

            LanguageDto languageDto = _mapper.Map<FilmMetadata, LanguageDto>(filmMetadata);
            return ExecResult<LanguageDto>.Success("Language updated successfully.", languageDto);
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
