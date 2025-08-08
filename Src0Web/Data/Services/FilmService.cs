using Data.BLL;
using Data.DAL;
using Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Shared.Result;

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
            List<FilmMetaLink> links = await _filmMetaLinkDao.GetManyByFilmIdAsync(filmId);
            if (links == null || links.Count == 0)
                return new List<FilmMetadata>();

            List<int> metaIds = links.Select(s => s.MetaId).ToList();
            List<FilmMetadata> metadata = await _filmMetadataDao.GetsByIdsAsync(metaIds);

            return metadata;
        }

        private async Task<List<Taxonomy>> GetTaxonomiesByFilmIdAsync(string filmId)
        {
            List<TaxonomyLink> links = await _taxonomyLinkDao.GetManyByFilmIdAsync(filmId);
            if (links == null || links.Count == 0)
                return new List<Taxonomy>();

            List<int> taxonomyIds = links.Select(s => s.TaxonomyId).ToList();
            List<Taxonomy> taxonomies = await _taxonomyDao.GetsByIdsAsync(taxonomyIds);
            return taxonomies;
        }

        private async Task<List<People>> GetPeoplesByFilmIdAsync(string filmId)
        {
            List<PeopleLink> links = await _peopleLinkDao.GetManyByFilmIdAsync(filmId);
            if (links == null || links.Count == 0)
                return new List<People>();

            List<long> personIds = links.Select(s => s.PersonId).ToList();
            List<People> peoples = await _generalDao.PeopleDao.GetsByIdsAsync(personIds);
            return peoples;
        }

        public async Task<PagedList<FilmDto>> GetFilmsAsync(long pageIndex = 1, long pageSize = 10)
        {
            long skip = (pageIndex - 1) * pageSize;
            List<Film> films = await _filmDao.GetsAsync(skip, take: pageSize);

            List<FilmDto> filmDtos = films.Select(s => new FilmDto
            {
                ID = s.ID,
                Name = s.Name,
                Description = s.Description,
                Thumbnail = s.Thumbnail,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList();

            long totalItems = await _filmDao.CountAsync();

            return new PagedList<FilmDto>
            {
                Items = filmDtos,
                PageSize = pageSize,
                CurrentPage = pageIndex,
                TotalItems = totalItems,
            };
        }

        public async Task<PagedList<FilmDto>> GetFilmsByCategoryIdAsync(int categoryId, long pageIndex = 1, long pageSize = 10)
        {
           PagedList<Film> data = await _filmDao.GetsByCategoryIdAsync(categoryId, pageIndex, pageSize);

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
                PageSize = data.PageSize,
                CurrentPage = data.CurrentPage,
            };
        }

        public async Task<ExecResult<FilmDto>> AddFilmAsync(CreateFilmDto film)
        {
            if (string.IsNullOrEmpty(film.Name) || string.IsNullOrEmpty(film.ProductionCompany) || string.IsNullOrEmpty(film.ReleaseDate))
            {
                return new ExecResult<FilmDto>
                {
                    Status = ExecStatus.Invalid,
                    Message = "Film name, production company, and release date are required."
                };
            }

            Film newFilm = new Film
            {
                ID = Guid.NewGuid().ToString(),
                Name = film.Name,
                Description = film.Description,
                ProductionCompany = film.ProductionCompany,
                Thumbnail = film.Thumbnail,
                ReleaseDate = film.ReleaseDate,
                Source = film.Source
            };

            int affected = await _filmDao.AddAsync(newFilm);
            if (affected <= 0)
                return new ExecResult<FilmDto> { Status = ExecStatus.Failure, Message = "Failed to add film." };

            return new ExecResult<FilmDto>
            {
                Status = ExecStatus.Success,
                Message = "Film added successfully.",
                Data = new FilmDto
                {
                    ID = newFilm.ID,
                    Name = newFilm.Name,
                    Description = newFilm.Description,
                    ProductionCompany = newFilm.ProductionCompany,
                    Thumbnail = newFilm.Thumbnail,
                    ReleaseDate = newFilm.ReleaseDate,
                    CreatedAt = newFilm.CreatedAt
                }
            };
        }

        public async Task<ExecResult<FilmDto>> UpdateFilmAsync(UpdateFilmDto film)
        {
            throw new NotImplementedException();
        }

        public async Task<ExecResult> DeleteAsync(string id, bool forceDelete = false)
        {
            Film existingFilm = await _filmDao.GetAsync(id);
            if (existingFilm == null)
                return new ExecResult { Status = ExecStatus.NotFound, Message = "Film not found." };

            int affected = 0;

            if (!forceDelete)
            {
                if (existingFilm.DeletedAt != null)
                    return new ExecResult { Status = ExecStatus.Invalid, Message = "Film already deleted." };

                existingFilm.DeletedAt = DateTime.Now;
                affected = await _filmDao.UpdateAsync(existingFilm);
                if (affected <= 0)
                    return new ExecResult { Status = ExecStatus.Failure, Message = "Failed to mark film as deleted." };

                return new ExecResult { Status = ExecStatus.Success, Message = "Film marked as deleted successfully." };
            }

            _generalDao.Context.BeginTransaction();

            try
            {
                _ = await _filmMetaLinkDao.DeleteManyByFilmIdAsync(id);
                _ = await _taxonomyLinkDao.DeleteManyByFilmIdAsync(id);
                _ = await _peopleLinkDao.DeleteManyByFilmIdAsync(id);

                affected = await _filmDao.DeleteAsync(id);

                if (affected <= 0)
                {
                    _generalDao.Context.RollbackTransaction();
                    return new ExecResult { Status = ExecStatus.Failure, Message = "Failed to delete film and its related data." };
                }

                _generalDao.Context.CommitTransaction();

                return new ExecResult { Status = ExecStatus.Success, Message = "Country deleted successfully." };
            }
            catch (Exception ex)
            {
                _generalDao.Context.RollbackTransaction();
                throw new Exception($"An error occurred while deleting film: {ex.Message}", ex);
            }
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