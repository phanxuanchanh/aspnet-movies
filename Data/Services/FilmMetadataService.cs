using Common;
using Common.Web;
using Data.BLL;
using Data.DAL;
using Data.DTO;
using MSSQL.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Services
{
    public class FilmMetadataService
    {
        private readonly GeneralDao _generalDao;
        private readonly FilmMetadataDao _filmMetadataDao;

        public FilmMetadataService(GeneralDao generalDao) { 
            _generalDao = generalDao;
            _filmMetadataDao = _generalDao.FilmMetadataDao;
        }

        public async Task<PagedList<CountryDto>> GetCountriesAsync(int id)
        {
            SqlPagedList<FilmMetadata> pagedList = null;
            Expression<Func<Country, object>> orderBy = c => new { c.ID };

            List<FilmMetadata> filmMetadata =  await _filmMetadataDao.GetsAsync("country");

            List<CountryDto> countries = filmMetadata.Select(s => new CountryDto
            {
                ID = s.ID,
                Name = s.Name,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList();

            return null;
        }

        public async Task<List<CountryDto>> GetLanguagesAsync(int id)
        {
            List<FilmMetadata> filmMetadata = await _filmMetadataDao.GetsAsync("languages");

            return filmMetadata.Select(s => new CountryDto
            {
                ID = s.ID,
                Name = s.Name,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList();
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

            await _filmMetadataDao.AddAsync(filmMetadata);

            return new ExecResult<CountryDto> { 
                Status = ExecStatus.Success,
                Message = "Country added successfully.",
                Data = new CountryDto
                {
                    ID = filmMetadata.ID,
                    Name = filmMetadata.Name,
                    Description = filmMetadata.Description,
                    CreatedAt = filmMetadata.CreatedAt,
                    UpdatedAt = filmMetadata.UpdatedAt
                }
            };
        }


    }
}
