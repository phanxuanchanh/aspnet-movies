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
    public class FilmService : IDisposable
    {
        private readonly GeneralDao _generalDao;
        private readonly FilmMetadataDao _filmMetadataDao;
        private readonly FilmMetaLinkDao _filmMetaLinkDao;
        private readonly TaxonomyDao _taxonomyDao;
        private readonly TaxonomyLinkDao _taxonomyLinkDao;
        private readonly PeopleDao _peopleDao;
        private readonly PeopleLinkDao _peopleLinkDao;
        private readonly FilmDao _filmDao;
        private bool disposedValue;

        public FilmService(GeneralDao generalDao)
        {
            _generalDao = generalDao;
            _filmMetadataDao = _generalDao.FilmMetadataDao;
            _filmMetaLinkDao = _generalDao.FilmMetaLinkDao;
            _taxonomyDao = _generalDao.TaxonomyDao;
            _taxonomyLinkDao = _generalDao.TaxonomyLinkDao;
            _peopleDao = _generalDao.PeopleDao;
            _peopleLinkDao = _generalDao.PeopleLinkDao;
            _filmDao = _generalDao.FilmDao;
        }

        public async Task<ExecResult<FilmDto>> GetFilmAsync(string id, bool includeMetadata = true, bool includeTaxonomy = true, bool includePeople = true)
        {
            Film film = await _filmDao.GetAsync(id);
            if (film == null)
                return new ExecResult<FilmDto> { Status = ExecStatus.NotFound, Message = "Director not found." };

            FilmDto filmDto = new FilmDto
            {
                ID = film.ID,
                Name = film.Name,
                Description = film.Description,
                ProductionCompany = film.ProductionCompany,
                ReleaseDate = film.ReleaseDate,
                Upvote = film.Upvote,
                Downvote = film.Downvote,
                Views = film.Views,
                CreatedAt = film.CreatedAt,
            };

            if (includeMetadata)
            {
                List<FilmMetadata> metadata = await GetMetadataByFilmIdAsync(film.ID);
                filmDto.Language = metadata.Where(x => x.Type == "language").Select(s => new LanguageDto
                {
                    ID = s.Id,
                    Name = s.Name,
                    Description = s.Description
                }).FirstOrDefault();

                filmDto.Country = metadata.Where(x => x.Type == "country").Select(s => new CountryDto
                {
                    ID = s.Id,
                    Name = s.Name,
                    Description = s.Description
                }).FirstOrDefault();

                if (filmDto.Language == null)
                    filmDto.Language = new LanguageDto();

                if (filmDto.Country == null)
                    filmDto.Country = new CountryDto();
            }

            if (includeTaxonomy)
            {
                List<Taxonomy> taxonomies = await GetTaxonomiesByFilmIdAsync(film.ID);
                filmDto.Tags = taxonomies.Where(x => x.Type == "tag").Select(s => new TagDto
                {
                    ID = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                }).ToList();

                filmDto.Categories = taxonomies.Where(x => x.Type == "category").Select(s => new CategoryDto
                {
                    ID = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                }).ToList();

                if (filmDto.Tags == null)
                    filmDto.Tags = new List<TagDto>();

                if (filmDto.Categories == null)
                    filmDto.Categories = new List<CategoryDto>();
            }

            if (includePeople)
            {
                List<People> peoples = await GetPeoplesByFilmIdAsync(film.ID);
                filmDto.Directors = peoples.Where(x => x.Type == "director").Select(s => new DirectorDto
                {
                    ID = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                }).ToList();

                filmDto.Actors = peoples.Where(x => x.Type == "actor").Select(s => new ActorDto
                {
                    ID = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                }).ToList();

                if (filmDto.Directors == null)
                    filmDto.Directors = new List<DirectorDto>();

                if (filmDto.Actors == null)
                    filmDto.Actors = new List<ActorDto>();
            }

            return new ExecResult<FilmDto>
            {
                Status = ExecStatus.Success,
                Message = "People retrieved successfully.",
                Data = filmDto
            };
        }

        private async Task<List<FilmMetadata>> GetMetadataByFilmIdAsync(string filmId)
        {
            List<FilmMetaLink> links = await _filmMetaLinkDao.GetsByFilmIdAsync(filmId);
            if (links == null || links.Count == 0)
                return new List<FilmMetadata>();

            List<int> metaIds = links.Select(s => s.MetaId).ToList();
            List<FilmMetadata> metadata = await _filmMetadataDao.GetsByIdsAsync(metaIds);

            return metadata;
        }

        private async Task<List<Taxonomy>> GetTaxonomiesByFilmIdAsync(string filmId)
        {
            List<TaxonomyLink> links = await _taxonomyLinkDao.GetsByFilmIdAsync(filmId);
            if (links == null || links.Count == 0)
                return new List<Taxonomy>();

            List<int> taxonomyIds = links.Select(s => s.TaxonomyId).ToList();
            List<Taxonomy> taxonomies = await _taxonomyDao.GetsByIdsAsync(taxonomyIds);
            return taxonomies;
        }

        private async Task<List<People>> GetPeoplesByFilmIdAsync(string filmId)
        {
            List<PeopleLink> links = await _peopleLinkDao.GetsByFilmIdAsync(filmId);
            if (links == null || links.Count == 0)
                return new List<People>();

            List<long> personIds = links.Select(s => s.PersonId).ToList();
            List<People> peoples = await _generalDao.PeopleDao.GetsByIdsAsync(personIds);
            return peoples;
        }

        public async Task<PagedList<FilmDto>> GetFilmsAsync(long pageIndex = 1, long pageSize = 10)
        {
            SqlPagedList<Film> data = await _filmDao.GetsAsync(pageIndex, pageSize);

            List<FilmDto> films = data.Items.Select(s => new FilmDto
            {
                ID = s.ID,
                Name = s.Name,
                Description = s.Description,
                Thumbnail = s.Thumbnail,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList();

            return new PagedList<FilmDto>
            {
                Items = films,
                PageNumber = data.PageNumber,
                CurrentPage = data.CurrentPage,
            };
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

        public async Task<ExecResult> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}