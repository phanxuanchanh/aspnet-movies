using Data.Models;
using MSSQL.Access;
using MSSQL.Connection;

namespace Data.DAL
{
    public class DBContext : SqlContext
    {
        private bool disposed;
        private SqlAccess<AppSetting> appSettings;
        private SqlAccess<Role> roles;
        private SqlAccess<User> users;
        private SqlAccess<PaymentMethod> paymentMethods;
        private SqlAccess<PaymentInfo> paymentInfos;
        private SqlAccess<Film> films;
        private SqlAccess<UserReaction> userReactions;
        private SqlAccess<FilmMetadata> filmMetadata;
        private SqlAccess<FilmMetaLink> filmMetaLinks;
        private SqlAccess<People> people;
        private SqlAccess<PeopleLink> peopleLinks;
        private SqlAccess<Taxonomy> taxonomies;
        private SqlAccess<TaxonomyLink> taxonomyLinks;

        public DBContext()
            : base()
        {
            appSettings = null;
            roles = null;
            users = null;
            paymentInfos = null;
            paymentMethods = null;
            films = null;
            userReactions = null;
            filmMetadata = null;
            filmMetaLinks = null;
            people = null;
            peopleLinks = null;
            taxonomies = null;
            taxonomyLinks = null;
            disposed = false;
        }

        public SqlAccess<AppSetting> AppSettings { get { return InitSqlAccess<AppSetting>(ref appSettings); } }
        public SqlAccess<Role> Roles { get { return InitSqlAccess<Role>(ref roles); } }
        public SqlAccess<User> Users { get { return InitSqlAccess<User>(ref users); } }
        public SqlAccess<PaymentMethod> PaymentMethods { get { return InitSqlAccess<PaymentMethod>(ref paymentMethods); } }
        public SqlAccess<PaymentInfo> PaymentInfos { get { return InitSqlAccess<PaymentInfo>(ref paymentInfos); } }
        public SqlAccess<Film> Films { get { return InitSqlAccess<Film>(ref films); } }
        public SqlAccess<UserReaction> UserReactions { get { return InitSqlAccess<UserReaction>(ref userReactions); } }
        public SqlAccess<FilmMetadata> FilmMetadata { get { return InitSqlAccess<FilmMetadata>(ref filmMetadata); } }
        public SqlAccess<FilmMetaLink> FilmMetaLinks { get { return InitSqlAccess<FilmMetaLink>(ref filmMetaLinks); } }
        public SqlAccess<People> People { get { return InitSqlAccess<People>(ref people); } }
        public SqlAccess<PeopleLink> PeopleLinks { get { return InitSqlAccess<PeopleLink>(ref peopleLinks); } }
        public SqlAccess<Taxonomy> Taxonomies { get { return InitSqlAccess<Taxonomy>(ref taxonomies); } }
        public SqlAccess<TaxonomyLink> TaxonomyLinks { get { return InitSqlAccess<TaxonomyLink>(ref taxonomyLinks); } }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                try
                {
                    if (disposing)
                    {
                        DisposeSqlAccess<Role>(ref roles);
                        DisposeSqlAccess<User>(ref users);
                        DisposeSqlAccess<PaymentMethod>(ref paymentMethods);
                        DisposeSqlAccess<PaymentInfo>(ref paymentInfos);
                        DisposeSqlAccess<Film>(ref films);
                        DisposeSqlAccess<UserReaction>(ref userReactions);
                        DisposeSqlAccess<FilmMetadata>(ref filmMetadata);
                        DisposeSqlAccess<FilmMetaLink>(ref filmMetaLinks);
                        DisposeSqlAccess<People>(ref people);
                        DisposeSqlAccess<PeopleLink>(ref peopleLinks);
                        DisposeSqlAccess<Taxonomy>(ref taxonomies);
                        DisposeSqlAccess<TaxonomyLink>(ref taxonomyLinks);
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
