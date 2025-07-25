﻿using Data.DAL;
using MSSQL.Access;
using MSSQL.Query;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Common.Web;

namespace Data.BLL
{
    public class PeopleDao
    {
        private readonly DBContext _context;

        internal PeopleDao(DBContext context)
        {
            _context = context;
        }

        public async Task<People> GetAsync(long id)
        {
            return await _context.People.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<People>> GetsByIdsAsync(List<long> ids)
        {
            StringBuilder commandTextBuilder = new StringBuilder("SELECT * FROM People WHERE Id IN (");
            SqlParameter[] parameters = new SqlParameter[ids.Count];
            for (int i = 0; i < ids.Count; i++)
            {
                commandTextBuilder.Append($"@Id{i}, ");
                parameters[i] = new SqlParameter($"@Id{i}", ids[i]);
            }
            commandTextBuilder.Append(")").Replace(", )", " )");

            return await _context.Execute_ToListAsync<People>(commandTextBuilder.ToString(), CommandType.Text, parameters);
        }

        public async Task<PagedList<People>> GetsAsync(string type = "director", long pageIndex = 1, long pageSize = 10)
        {
            long skip = (pageIndex - 1) * pageSize;
            List<People> people = await _context.People
                .Where(x => x.Type == type).OrderBy(o => new { o.Id }).ToListAsync();

            long count = await _context.People.CountAsync(x => x.Type == type);

            return new PagedList<People>
            {
                Items = people,
                CurrentPage = pageIndex,
            };
        }

        public async Task<int> AddAsync(People people)
        {
            people.CreatedAt = DateTime.Now;

            return await _context.People.InsertAsync(people, new List<string> { "Id", "UpdatedAt", "DeletedAt" });
        }

        public async Task<int> UpdateAsync(People people)
        {
            people.UpdatedAt = DateTime.Now;
            return await _context.People
                .Where(x => x.Id == people.Id)
                .UpdateAsync(people, s => new { s.Name, s.Description, s.UpdatedAt });
        }

        public async Task<int> DeleteAsync(long id, bool forceDelete = false)
        {
            People people = await GetAsync(id);
            if (people == null)
                return 0;

            if (forceDelete)
                return await _context.People.DeleteAsync(x => x.Id == id);

            people.DeletedAt = DateTime.Now;
            return await _context.People
                .Where(x => x.Id == id)
                .UpdateAsync(people, s => new { s.DeletedAt });
        }
    }
}