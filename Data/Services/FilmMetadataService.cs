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
    public class FilmMetadataService : IDisposable
    {
        private readonly GeneralDao _generalDao;
        private readonly FilmMetadataDao _filmMetadataDao;
        private bool disposedValue;

        public FilmMetadataService(GeneralDao generalDao) { 
            _generalDao = generalDao;
            _filmMetadataDao = _generalDao.FilmMetadataDao;
        }

        public async Task<ExecResult<CountryDto>> GetCountryAsync(int id)
        {
            FilmMetadata metadata = await _filmMetadataDao.GetAsync(id);
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

        public async Task<PagedList<CountryDto>> GetCountriesAsync(long pageIndex = 1, long pageSize = 10)
        {
            SqlPagedList<FilmMetadata> data =  await _filmMetadataDao.GetsAsync("country", pageIndex, pageSize);

            List<CountryDto> countries = data.Items.Select(s => new CountryDto
            {
                ID = s.Id,
                Name = s.Name,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList();

            return new PagedList<CountryDto>
            {
                Items = countries,
                PageNumber = data.PageNumber,
                CurrentPage = data.CurrentPage,
            };
        }

        public async Task<PagedList<LanguageInfo>> GetLanguagesAsync(long pageIndex = 1, long pageSize = 10)
        {
            SqlPagedList<FilmMetadata> data = await _filmMetadataDao.GetsAsync("country", pageIndex, pageSize);

            List<LanguageInfo> languages = data.Items.Select(s => new LanguageInfo
            {
                
            }).ToList();

            return new PagedList<LanguageInfo>
            {
                Items = languages,
                PageNumber = data.PageNumber,
                CurrentPage = data.CurrentPage,
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

        public async Task<ExecResult> DeleteAsync(int id, bool forceDelete = false)
        {
            int affected = await _filmMetadataDao.DeleteAsync(id, forceDelete);
            if (affected <= 0)
                return new ExecResult { Status = ExecStatus.NotFound, Message = "Country not found or deletion failed." };
            
            return new ExecResult { Status = ExecStatus.Success, Message = "Country deleted successfully." };
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }

                _generalDao?.Dispose();
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
