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
    public class PeopleService : IDisposable
    {
        private readonly GeneralDao _generalDao;
        private readonly PeopleDao _peopleDao;
        private bool disposedValue;

        public PeopleService(GeneralDao generalDao) {
            _generalDao = generalDao;
            _peopleDao = _generalDao.PeopleDao;
        }

        public async Task<ExecResult<DirectorDto>> GetDirectorAsync(long id)
        {
            People people = await _peopleDao.GetAsync(id);
            if (people == null)
                return new ExecResult<DirectorDto> { Status = ExecStatus.NotFound, Message = "Director not found." };

            return new ExecResult<DirectorDto>
            {
                Status = ExecStatus.Success,
                Message = "People retrieved successfully.",
                Data = new DirectorDto
                {
                    ID = people.Id,
                    Name = people.Name,
                    Description = people.Description,
                    CreatedAt = people.CreatedAt,
                    UpdatedAt = people.UpdatedAt
                }
            };
        }

        public async Task<ExecResult<ActorDto>> GetActorAsync(long id)
        {
            People people = await _peopleDao.GetAsync(id);
            if (people == null)
                return new ExecResult<ActorDto> { Status = ExecStatus.NotFound, Message = "Actor not found." };

            return new ExecResult<ActorDto>
            {
                Status = ExecStatus.Success,
                Message = "People retrieved successfully.",
                Data = new ActorDto
                {
                    ID = people.Id,
                    Name = people.Name,
                    Description = people.Description,
                    CreatedAt = people.CreatedAt,
                    UpdatedAt = people.UpdatedAt
                }
            };
        }

        public async Task<PagedList<DirectorDto>> GetDirectorsAsync(long pageIndex = 1, long pageSize = 10)
        {
            SqlPagedList<People> data = await _peopleDao.GetsAsync("director", pageIndex, pageSize);

            List<DirectorDto> countries = data.Items.Select(s => new DirectorDto
            {
                ID = s.Id,
                Name = s.Name,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList();

            return new PagedList<DirectorDto>
            {
                Items = countries,
                PageNumber = data.PageNumber,
                CurrentPage = data.CurrentPage,
            };
        }

        public async Task<PagedList<ActorDto>> GetActorsAsync(long pageIndex = 1, long pageSize = 10)
        {
            SqlPagedList<People> data = await _peopleDao.GetsAsync("actor", pageIndex, pageSize);

            List<ActorDto> actors = data.Items.Select(s => new ActorDto
            {
                ID = s.Id,
                Name = s.Name,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList();

            return new PagedList<ActorDto>
            {
                Items = actors,
                PageNumber = data.PageNumber,
                CurrentPage = data.CurrentPage,
            };
        }

        public async Task<ExecResult<DirectorDto>> AddDirectorAsync(CreateDirectorDto input)
        {
            if (string.IsNullOrEmpty(input.Name))
                return new ExecResult<DirectorDto> { Status = ExecStatus.Invalid, Message = "Name is required." };

            People person = new People
            {
                Name = input.Name,
                Description = input.Description,
                Type = "director",
                CreatedAt = DateTime.Now,
            };

            int affected = await _peopleDao.AddAsync(person);
            if (affected <= 0)
                return new ExecResult<DirectorDto> { Status = ExecStatus.Failure, Message = "Failed to add director." };

            return new ExecResult<DirectorDto>
            {
                Status = ExecStatus.Success,
                Message = "Director added successfully.",
                Data = new DirectorDto
                {
                    ID = person.Id,
                    Name = person.Name,
                    Description = person.Description,
                    CreatedAt = person.CreatedAt,
                    UpdatedAt = person.UpdatedAt
                }
            };
        }

        public async Task<ExecResult<ActorDto>> AddActorAsync(CreateActorDto input)
        {
            if (string.IsNullOrEmpty(input.Name))
                return new ExecResult<ActorDto> { Status = ExecStatus.Invalid, Message = "Name is required." };

            People person = new People
            {
                Name = input.Name,
                Description = input.Description,
                Type = "actor",
                CreatedAt = DateTime.Now,
            };

            int affected = await _peopleDao.AddAsync(person);
            if (affected <= 0)
                return new ExecResult<ActorDto> { Status = ExecStatus.Failure, Message = "Failed to add actor." };

            return new ExecResult<ActorDto>
            {
                Status = ExecStatus.Success,
                Message = "Actor added successfully.",
                Data = new ActorDto
                {
                    ID = person.Id,
                    Name = person.Name,
                    Description = person.Description,
                    CreatedAt = person.CreatedAt,
                    UpdatedAt = person.UpdatedAt
                }
            };
        }

        public async Task<ExecResult<DirectorDto>> UpdateDirectorAsync(UpdateDirectorDto input)
        {
            if (string.IsNullOrEmpty(input.Name))
                return new ExecResult<DirectorDto> { Status = ExecStatus.Invalid, Message = "Name is required." };

            People person = new People
            {
                Id = input.ID,
                Name = input.Name,
                Description = input.Description,
                Type = "director",
                CreatedAt = DateTime.Now,
            };

            int affected = await _peopleDao.UpdateAsync(person);
            if (affected <= 0)
                return new ExecResult<DirectorDto> { Status = ExecStatus.Failure, Message = "Failed to update director." };

            return new ExecResult<DirectorDto>
            {
                Status = ExecStatus.Success,
                Message = "Director updated successfully.",
                Data = new DirectorDto
                {
                    ID = person.Id,
                    Name = person.Name,
                    Description = person.Description,
                    CreatedAt = person.CreatedAt,
                    UpdatedAt = person.UpdatedAt
                }
            };
        }

        public async Task<ExecResult<ActorDto>> UpdateActorAsync(UpdateActorDto input)
        {
            if (string.IsNullOrEmpty(input.Name))
                return new ExecResult<ActorDto> { Status = ExecStatus.Invalid, Message = "Name is required." };

            People person = new People
            {
                Id = input.ID,
                Name = input.Name,
                Description = input.Description,
                Type = "actor",
                CreatedAt = DateTime.Now,
            };

            int affected = await _peopleDao.UpdateAsync(person);
            if (affected <= 0)
                return new ExecResult<ActorDto> { Status = ExecStatus.Failure, Message = "Failed to update director." };

            return new ExecResult<ActorDto>
            {
                Status = ExecStatus.Success,
                Message = "Actor updated successfully.",
                Data = new ActorDto
                {
                    ID = person.Id,
                    Name = person.Name,
                    Description = person.Description,
                    CreatedAt = person.CreatedAt,
                    UpdatedAt = person.UpdatedAt
                }
            };
        }

        public async Task<ExecResult> DeleteAsync(long id, bool forceDelete = false)
        {
            int affected = await _peopleDao.DeleteAsync(id, forceDelete);
            if (affected <= 0)
                return new ExecResult { Status = ExecStatus.NotFound, Message = "People not found or deletion failed." };

            return new ExecResult { Status = ExecStatus.Success, Message = "People deleted successfully." };
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