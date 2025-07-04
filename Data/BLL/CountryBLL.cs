using Common.Web;
using Data.DAL;
using Data.DTO;
using MSSQL.Access;
using MSSQL.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.BLL
{
    public class CountryBLL : BusinessLogicLayer
    {
        private bool disposed;
        private bool includeDescription;

        public bool IncludeDescription { set { includeDescription = value; } }

        public CountryBLL()
            : base()
        {
            InitDAL();
            SetDefault();
            disposed = false;
        }

        public CountryBLL(BusinessLogicLayer bll)
            : base()
        {
            InitDAL(bll.db);
            SetDefault();
            disposed = false;
        }

        public override void SetDefault()
        {
            base.SetDefault();
            includeDescription = false;
        }

        private CountryInfo ToCountryInfo(Country country)
        {
            if (country == null)
                return null;

            CountryInfo countryInfo = new CountryInfo();
            countryInfo.ID = country.ID;
            countryInfo.name = country.Name;

            if (includeDescription)
                countryInfo.description = country.Description;

            if (includeTimestamp)
            {
                countryInfo.createAt = country.CreatedAt;
                countryInfo.updateAt = country.UpdatedAt;
            }

            return countryInfo;
        }

        private Country ToCountry(CountryCreation countryCreation)
        {
            if (countryCreation == null)
                throw new Exception("@'countryCreation' must not be null");

            return new Country
            {
                Name = countryCreation.name,
                Description = countryCreation.description,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }

        private Country ToCountry(CountryUpdate countryUpdate)
        {
            if (countryUpdate == null)
                throw new Exception("@'countryUpdate' must not be null");

            return new Country
            {
                ID = countryUpdate.ID,
                Name = countryUpdate.name,
                Description = countryUpdate.description,
                UpdatedAt = DateTime.Now
            };
        }

        public async Task<List<CountryInfo>> GetCountriesAsync()
        {
            List<CountryInfo> countries = null;
            if (includeDescription && includeTimestamp)
                countries = (await db.Countries.ToListAsync()).Select(c => ToCountryInfo(c)).ToList();
            else if(includeDescription)
                countries = (await db.Countries.ToListAsync(c => new { c.ID, c.Name, c.Description }))
                    .Select(c => ToCountryInfo(c)).ToList();
            else if(includeTimestamp)
                countries = (await db.Countries.ToListAsync(c => new { c.ID, c.Name, c.CreatedAt, c.UpdatedAt }))
                    .Select(c => ToCountryInfo(c)).ToList();
            else
                countries = (await db.Countries.ToListAsync(c => new { c.ID, c.Name }))
                    .Select(c => ToCountryInfo(c)).ToList();

            return countries;
        }

        public List<CountryInfo> GetCountries()
        {
            List<CountryInfo> countries = null;
            if (includeDescription && includeTimestamp)
                countries = db.Countries.ToList().Select(c => ToCountryInfo(c)).ToList();
            else if(includeDescription)
                countries = db.Countries.ToList(c => new { c.ID, c.Name, c.Description })
                    .Select(c => ToCountryInfo(c)).ToList();
            else if(includeTimestamp)
                countries = db.Countries.ToList(c => new { c.ID, c.Name, c.CreatedAt, c.UpdatedAt })
                    .Select(c => ToCountryInfo(c)).ToList();
            else
                countries = db.Countries.ToList(c => new { c.ID, c.Name })
                    .Select(c => ToCountryInfo(c)).ToList();

            return countries;
        }

        public async Task<PagedList<CountryInfo>> GetCountriesAsync(int pageIndex, int pageSize)
        {
            SqlPagedList<Country> pagedList = null;
            Expression<Func<Country, object>> orderBy = c => new { c.ID };
            if (includeDescription && includeTimestamp)
                pagedList = await db.Countries.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if (includeDescription)
                pagedList = await db.Countries.ToPagedListAsync(
                    c => new { c.ID, c.Name, c.Description }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if(includeTimestamp)
                pagedList = await db.Countries.ToPagedListAsync(
                    c => new { c.ID, c.Name, c.CreatedAt, c.UpdatedAt }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = await db.Countries.ToPagedListAsync(
                    c => new { c.ID, c.Name }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);

            return new PagedList<CountryInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToCountryInfo(c)).ToList()
            };
        }

        public PagedList<CountryInfo> GetCountries(int pageIndex, int pageSize)
        {
            SqlPagedList<Country> pagedList = null;
            Expression<Func<Country, object>> orderBy = c => new { c.ID };
            if (includeDescription && includeTimestamp)
                pagedList = db.Countries.ToPagedList(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if (includeDescription)
                pagedList = db.Countries.ToPagedList(
                    c => new { c.ID, c.Name, c.Description }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if (includeTimestamp)
                pagedList = db.Countries.ToPagedList(
                    c => new { c.ID, c.Name, c.CreatedAt, c.UpdatedAt }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = db.Countries.ToPagedList(
                    c => new { c.ID, c.Name }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);

            return new PagedList<CountryInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToCountryInfo(c)).ToList()
            };
        }

        public async Task<CountryInfo> GetCountryAsync(int countryId)
        {
            if (countryId <= 0)
                throw new Exception("@'countryId' must be greater than 0");

            Country country = null;
            if (includeDescription && includeTimestamp)
                country = (await db.Countries.SingleOrDefaultAsync(c => c.ID == countryId));
            else if (includeDescription)
                country = (await db.Countries
                    .SingleOrDefaultAsync(c => new { c.ID, c.Name, c.Description }, c => c.ID == countryId));
            else if (includeTimestamp)
                country = (await db.Countries
                    .SingleOrDefaultAsync(c => new { c.ID, c.Name, c.CreatedAt, c.UpdatedAt }, c => c.ID == countryId));
            else
                country = (await db.Countries
                    .SingleOrDefaultAsync(c => new { c.ID, c.Name }, c => c.ID == countryId));

            return ToCountryInfo(country);
        }

        public CountryInfo GetCountry(int countryId)
        {
            if (countryId <= 0)
                throw new Exception("@'countryId' must be greater than 0");

            Country country = null;
            if (includeDescription && includeTimestamp)
                country = db.Countries.SingleOrDefault(c => c.ID == countryId);
            else if (includeDescription)
                country = db.Countries
                    .SingleOrDefault(c => new { c.ID, c.Name, c.Description }, c => c.ID == countryId);
            else if (includeTimestamp)
                country = db.Countries
                    .SingleOrDefault(c => new { c.ID, c.Name, c.CreatedAt, c.UpdatedAt }, c => c.ID == countryId);
            else
                country = db.Countries
                    .SingleOrDefault(c => new { c.ID, c.Name }, c => c.ID == countryId);

            return ToCountryInfo(country);
        }

        public async Task<CreationState> CreateCountryAsync(CountryCreation countryCreation)
        {
            Country country = ToCountry(countryCreation);
            if (country.Name == null)
                throw new Exception("@'country.name' must not be null");

            int checkExists = (int)await db.Countries.CountAsync(c => c.Name == country.Name);
            if (checkExists != 0)
                return CreationState.AlreadyExists;

            int affected;
            if (country.Description == null)
                affected = await db.Countries.InsertAsync(country, new List<string> { "ID", "description" });
            else
                affected = await db.Countries.InsertAsync(country, new List<string> { "ID" });

            return (affected == 0) ? CreationState.Failed : CreationState.Success;
        }

        public async Task<UpdateState> UpdateCountryAsync(CountryUpdate countryUpdate)
        {
            Country country = ToCountry(countryUpdate);
            if (country.Name == null)
                throw new Exception("@'country.name' must not be null");

            int affected;
            if (country.Description == null)
                affected = await db.Countries.UpdateAsync(
                    country,
                    c => new { c.Name, c.UpdatedAt },
                    c => c.ID == country.ID
                );
            else
                affected = await db.Countries.UpdateAsync(
                    country,
                    c => new { c.Name, c.Description, c.UpdatedAt },
                    c => c.ID == country.ID
                );

            return (affected == 0) ? UpdateState.Failed : UpdateState.Success;
        }

        public async Task<DeletionState> DeleteCountryAsync(int countryId)
        {
            if (countryId <= 0)
                throw new Exception("@'countryId' must be greater than");

            long filmNumberOfCountryId = await db.Films.CountAsync(f => f.countryId == countryId);
            if (filmNumberOfCountryId > 0)
                return DeletionState.ConstraintExists;

            int affected = await db.Countries.DeleteAsync(c => c.ID == countryId);
            return (affected == 0) ? DeletionState.Failed : DeletionState.Success;
        }

        public async Task<int> CountAllAsync()
        {
            return (int)await db.Countries.CountAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                try
                {
                    if (disposing)
                    {

                    }
                    this.disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
    }
}
