using Data.Models;
using MSSQL.Access;

namespace Data.DAL
{
    public class DBContext : SqlContext
    {
        public SqlAccess<AppSetting> AppSettings => InitSqlAccess<AppSetting>();
        public SqlAccess<Role> Roles => InitSqlAccess<Role>();
        public SqlAccess<User> Users => InitSqlAccess<User>();
        public SqlAccess<PaymentMethod> PaymentMethods => InitSqlAccess<PaymentMethod>();
        public SqlAccess<PaymentInfo> PaymentInfos => InitSqlAccess<PaymentInfo>();
        public SqlAccess<Film> Films => InitSqlAccess<Film>();
        public SqlAccess<UserReaction> UserReactions => InitSqlAccess<UserReaction>();
        public SqlAccess<FilmMetadata> FilmMetadata => InitSqlAccess<FilmMetadata>();
        public SqlAccess<FilmMetaLink> FilmMetaLinks => InitSqlAccess<FilmMetaLink>();
        public SqlAccess<People> People => InitSqlAccess<People>();
        public SqlAccess<PeopleLink> PeopleLinks => InitSqlAccess<PeopleLink>();
        public SqlAccess<Taxonomy> Taxonomies => InitSqlAccess<Taxonomy>();
        public SqlAccess<TaxonomyLink> TaxonomyLinks => InitSqlAccess<TaxonomyLink>();
    }
}
