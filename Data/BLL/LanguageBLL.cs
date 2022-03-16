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
    public class LanguageBLL : BusinessLogicLayer
    {
        private bool disposed;
        private bool includeDescription;

        public bool IncludeDescription { set { includeDescription = value; } }

        public LanguageBLL()
            : base()
        {
            InitDAL();
            SetDefault();
            disposed = false;
        }

        public LanguageBLL(BusinessLogicLayer bll)
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

        private LanguageInfo ToLanguageInfo(Language language)
        {
            if (language == null)
                return null;

            LanguageInfo languageInfo = new LanguageInfo();
            languageInfo.ID = language.ID;
            languageInfo.name = language.name;

            if (includeDescription)
                languageInfo.description = language.description;

            if (includeTimestamp)
            {
                languageInfo.createAt = language.createAt;
                languageInfo.updateAt = language.updateAt;
            }

            return languageInfo;
        }

        private Language ToLanguage(LanguageCreation languageCreation)
        {
            if (languageCreation == null)
                throw new Exception("@'languageCreation' must not be null");

            return new Language
            {
                name = languageCreation.name,
                description = languageCreation.description,
                createAt = DateTime.Now,
                updateAt = DateTime.Now,
            };
        }

        private Language ToLanguage(LanguageUpdate languageUpdate)
        {
            if (languageUpdate == null)
                throw new Exception("@'languageUpdate' must not be null");

            return new Language
            {
                ID = languageUpdate.ID,
                name = languageUpdate.name,
                description = languageUpdate.description,
                updateAt = DateTime.Now
            };
        }

        public async Task<List<LanguageInfo>> GetLanguagesAsync()
        {
            List<LanguageInfo> languages = null;
            if (includeDescription && includeTimestamp)
                languages = (await db.Languages.ToListAsync())
                    .Select(l => ToLanguageInfo(l)).ToList();
            else if (includeDescription)
                languages = (await db.Languages.ToListAsync(c => new { c.ID, c.name, c.description }))
                    .Select(l => ToLanguageInfo(l)).ToList();
            else if (includeTimestamp)
                languages = (await db.Languages.ToListAsync(c => new { c.ID, c.name, c.createAt, c.updateAt }))
                    .Select(l => ToLanguageInfo(l)).ToList();
            else
                languages = (await db.Languages.ToListAsync(c => new { c.ID, c.name }))
                    .Select(l => ToLanguageInfo(l)).ToList();

            return languages;
        }

        public List<LanguageInfo> GetLanguages()
        {
            List<LanguageInfo> languages = null;
            if (includeDescription && includeTimestamp)
                languages = db.Languages.ToList().Select(l => ToLanguageInfo(l)).ToList();
            else if (includeDescription)
                languages = db.Languages.ToList(c => new { c.ID, c.name, c.description })
                    .Select(l => ToLanguageInfo(l)).ToList();
            else if (includeTimestamp)
                languages = db.Languages.ToList(c => new { c.ID, c.name, c.createAt, c.updateAt })
                    .Select(l => ToLanguageInfo(l)).ToList();
            else
                languages = db.Languages.ToList(c => new { c.ID, c.name })
                    .Select(l => ToLanguageInfo(l)).ToList();

            return languages;
        }

        public async Task<PagedList<LanguageInfo>> GetLanguagesAsync(int pageIndex, int pageSize)
        {
            SqlPagedList<Language> pagedList = null;
            Expression<Func<Language, object>> orderBy = c => new { c.ID };
            if (includeDescription && includeTimestamp)
                pagedList = await db.Languages.ToPagedListAsync(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if (includeDescription)
                pagedList = await db.Languages.ToPagedListAsync(
                    c => new { c.ID, c.name, c.description }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if (includeTimestamp)
                pagedList = await db.Languages.ToPagedListAsync(
                    c => new { c.ID, c.name, c.createAt, c.updateAt }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = await db.Languages.ToPagedListAsync(
                    c => new { c.ID, c.name }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);

            return new PagedList<LanguageInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToLanguageInfo(c)).ToList()
            };
        }

        public PagedList<LanguageInfo> GetLanguages(int pageIndex, int pageSize)
        {
            SqlPagedList<Language> pagedList = null;
            Expression<Func<Language, object>> orderBy = c => new { c.ID };
            if (includeDescription && includeTimestamp)
                pagedList = db.Languages.ToPagedList(orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if (includeDescription)
                pagedList = db.Languages.ToPagedList(
                    c => new { c.ID, c.name, c.description }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else if (includeTimestamp)
                pagedList = db.Languages.ToPagedList(
                    c => new { c.ID, c.name, c.createAt, c.updateAt }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);
            else
                pagedList = db.Languages.ToPagedList(
                    c => new { c.ID, c.name }, orderBy, SqlOrderByOptions.Asc, pageIndex, pageSize);

            return new PagedList<LanguageInfo>
            {
                PageNumber = pagedList.PageNumber,
                CurrentPage = pagedList.CurrentPage,
                Items = pagedList.Items.Select(c => ToLanguageInfo(c)).ToList()
            };
        }

        public async Task<LanguageInfo> GetLanguageAsync(int languageId)
        {
            if (languageId <= 0)
                throw new Exception("@'languageId' must be greater than 0");

            Language language = null;
            if (includeDescription && includeTimestamp)
                language = (await db.Languages.SingleOrDefaultAsync(l => l.ID == languageId));
            else if(includeDescription)
                language = (await db.Languages
                    .SingleOrDefaultAsync(l => new { l.ID, l.name, l.description }, l => l.ID == languageId));
            else if(includeTimestamp)
                language = (await db.Languages
                    .SingleOrDefaultAsync(l => new { l.ID, l.name, l.createAt, l.updateAt }, l => l.ID == languageId));
            else
                language = (await db.Languages
                    .SingleOrDefaultAsync(l => new { l.ID, l.name }, l => l.ID == languageId));

            return ToLanguageInfo(language);
        }

        public LanguageInfo GetLanguage(int languageId)
        {
            if (languageId <= 0)
                throw new Exception("@'languageId' must be greater than 0");

            Language language = null;
            if (includeDescription && includeTimestamp)
                language = db.Languages.SingleOrDefault(l => l.ID == languageId);
            else if(includeDescription)
                language = db.Languages
                    .SingleOrDefault(l => new { l.ID, l.name, l.description }, l => l.ID == languageId);
            else if(includeTimestamp)
                language = db.Languages
                    .SingleOrDefault(l => new { l.ID, l.name, l.createAt, l.updateAt }, l => l.ID == languageId);
            else
                language = db.Languages
                    .SingleOrDefault(l => new { l.ID, l.name }, l => l.ID == languageId);

            return ToLanguageInfo(language);
        }

        public async Task<CreationState> CreateLanguageAsync(LanguageCreation languageCreation)
        {
            Language language = ToLanguage(languageCreation);
            if (language.name == null)
                throw new Exception("@'language.name' must not be null");

            int checkExists = (int)await db.Languages.CountAsync(l => l.name == language.name);
            if (checkExists != 0)
                return CreationState.AlreadyExists;

            int affected;
            if (language.description == null)
                affected = await db.Languages.InsertAsync(language, new List<string> { "ID", "description" });
            else
                affected = await db.Languages.InsertAsync(language, new List<string> { "ID" });

            return (affected == 0) ? CreationState.Failed : CreationState.Success;
        }

        public async Task<UpdateState> UpdateLanguageAsync(LanguageUpdate languageUpdate)
        {
            Language language = ToLanguage(languageUpdate);
            if (language.name == null)
                throw new Exception("@'language.name' must not be null");

            int affected;
            if (language.description == null)
                affected = await db.Languages.UpdateAsync(
                    language,
                    l => new { l.name, l.updateAt },
                    l => l.ID == language.ID
                );
            else
                affected = await db.Languages.UpdateAsync(
                    language,
                    l => new { l.name, l.description, l.updateAt },
                    l => l.ID == language.ID
                );

            return (affected == 0) ? UpdateState.Failed : UpdateState.Success;
        }

        public async Task<DeletionState> DeleteLanguageAsync(int languageId)
        {
            if (languageId <= 0)
                throw new Exception("@'languageId' must be greater than 0");

            long filmNumberOfLanguageId = await db.Films.CountAsync(f => f.languageId == languageId);
            if (filmNumberOfLanguageId > 0)
                return DeletionState.ConstraintExists;

            int affected = await db.Languages.DeleteAsync(l => l.ID == languageId);
            return (affected == 0) ? DeletionState.Failed : DeletionState.Success;
        }

        public async Task<int> CountAllAsync()
        {
            return (int)await db.Languages.CountAsync();
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
