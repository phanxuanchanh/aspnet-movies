using Data.Models;
using MSSQL.Access;
using System;

namespace Data.DAL
{
    public class DBContext : SqlContext
    {
        private bool disposed;

        public SqlAccess<AppSetting> AppSettings => InitSqlAccess<AppSetting>();
        public SqlAccess<Role> Roles => InitSqlAccess<Role>();
        public SqlAccess<User> Users => InitSqlAccess<User>();
        public SqlAccess<Film> Films => InitSqlAccess<Film>();
        public SqlAccess<FilmMetadata> FilmMetadata => InitSqlAccess<FilmMetadata>();
        public SqlAccess<FilmMetaLink> FilmMetaLinks => InitSqlAccess<FilmMetaLink>();
        public SqlAccess<People> People => InitSqlAccess<People>();
        public SqlAccess<PeopleLink> PeopleLinks => InitSqlAccess<PeopleLink>();
        public SqlAccess<Taxonomy> Taxonomies => InitSqlAccess<Taxonomy>();
        public SqlAccess<TaxonomyLink> TaxonomyLinks => InitSqlAccess<TaxonomyLink>();

        protected override void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {

            }

            disposed = true;
            base.Dispose(disposing);
        }
    }
}
