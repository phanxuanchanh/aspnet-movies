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
            countryInfo.name = country.name;

            if (includeDescription)
                countryInfo.description = country.description;

            if (includeTimestamp)
            {
                countryInfo.createAt = country.createAt;
                countryInfo.updateAt = country.updateAt;
            }

            return countryInfo;
        }

        private Country ToCountry(CountryCreation countryCreation)
        {
            if (countryCreation == null)
                throw new Exception("@'countryCreation' must not be null");

            return new Country
            {
                name = countryCreation.name,
                description = countryCreation.description,
                createAt = DateTime.Now,
                updateAt = DateTime.Now
            };
        }

        private Country ToCountry(CountryUpdate countryUpdate)
        {
            if (countryUpdate == null)
                throw new Exception("@'countryUpdate' must not be null");

            return new Country
            {
                ID = countryUpdate.ID,
                name = countryUpdate.name,
                description = countryUpdate.description,
                updateAt = DateTime.Now
            };
        }

        public async Task<List<CountryInfo>> GetCountriesAsync()
        {
            List<CountryInfo> countries = null;
            if (includeDescription && includeTimestamp)
                countries = (await db.Countries.ToListAsync()).Select(c => ToCountryInfo(c)).ToList();
            else if(includeDescription)
                countries = (await db.Countries.ToListAsync(c => new { c.ID, c.name, c.description }))
                    .Select(c => ToCountryInfo(c)).ToList();
            else if(includeTimestamp)
                countries = (await db.Countries.ToListAsync(c => new { c.ID, c.name, c.createAt, c.updateAt }))
                    .Select(c => ToCountryInfo(c)).ToList();
            else
                countries = (await db.Countries.ToListAsync(c => new { c.ID, c.name }))
                    .Select(c => ToCountryInfo(c)).ToList();

            return countries;
        }

        public List<CountryInfo> GetCountries()
        {
            List<CountryInfo> countries = null;
            if (includeDescription && includeTimestamp)
                countries = db.Countries.ToList().Select(c => ToCountryInfo(c)).ToList();
            else if(includeDescription)
                countries = db.Countries.ToList(c => new { c.ID, c.name, c.description })
                    .Select(c => ToCountryInfo(c)).ToList();
            else if(includeTimestamp)
                countries = db.Countries.ToList(c => new { c.ID, c.name, c.createAt, c.updateAt })
                    .Select(c => ToCountryInfo(c)).ToList();
            else
                countries = db.Countries.ToList(c => new { c.ID, c.name })
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
                    c => new { c.ID, c.name, c.description }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if(includeTimestamp)
                pagedList = await db.Countries.ToPagedListAsync(
                    c => new { c.ID, c.name, c.createAt, c.updateAt }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = await db.Countries.ToPagedListAsync(
                    c => new { c.ID, c.name }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);

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
                    c => new { c.ID, c.name, c.description }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if (includeTimestamp)
                pagedList = db.Countries.ToPagedList(
                    c => new { c.ID, c.name, c.createAt, c.updateAt }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = db.Countries.ToPagedList(
                    c => new { c.ID, c.name }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);

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
                    .SingleOrDefaultAsync(c => new { c.ID, c.name, c.description }, c => c.ID == countryId));
            else if (includeTimestamp)
                country = (await db.Countries
                    .SingleOrDefaultAsync(c => new { c.ID, c.name, c.createAt, c.updateAt }, c => c.ID == countryId));
            else
                country = (await db.Countries
                    .SingleOrDefaultAsync(c => new { c.ID, c.name }, c => c.ID == countryId));

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
                    .SingleOrDefault(c => new { c.ID, c.name, c.description }, c => c.ID == countryId);
            else if (includeTimestamp)
                country = db.Countries
                    .SingleOrDefault(c => new { c.ID, c.name, c.createAt, c.updateAt }, c => c.ID == countryId);
            else
                country = db.Countries
                    .SingleOrDefault(c => new { c.ID, c.name }, c => c.ID == countryId);

            return ToCountryInfo(country);
        }

        public async Task<CreationState> CreateCountryAsync(CountryCreation countryCreation)
        {
            Country country = ToCountry(countryCreation);
            if (country.name == null)
                throw new Exception("@'country.name' must not be null");

            int checkExists = (int)await db.Countries.CountAsync(c => c.name == country.name);
            if (checkExists != 0)
                return CreationState.AlreadyExists;

            int affected;
            if (country.description == null)
                affected = await db.Countries.InsertAsync(country, new List<string> { "ID", "description" });
            else
                affected = await db.Countries.InsertAsync(country, new List<string> { "ID" });

            return (affected == 0) ? CreationState.Failed : CreationState.Success;
        }

        public async Task<UpdateState> UpdateCountryAsync(CountryUpdate countryUpdate)
        {
            Country country = ToCountry(countryUpdate);
            if (country.name == null)
                throw new Exception("@'country.name' must not be null");

            int affected;
            if (country.description == null)
                affected = await db.Countries.UpdateAsync(
                    country,
                    c => new { c.name, c.updateAt },
                    c => c.ID == country.ID
                );
            else
                affected = await db.Countries.UpdateAsync(
                    country,
                    c => new { c.name, c.description, c.updateAt },
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
