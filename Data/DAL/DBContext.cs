using MSSQL.Access;
using MSSQL.Connection;

namespace Data.DAL
{
    public class DBContext : SqlContext
    {
        private bool disposed;
        private SqlAccess<Role> roles;
        private SqlAccess<User> users;
        private SqlAccess<PaymentMethod> paymentMethods;
        private SqlAccess<PaymentInfo> paymentInfos;
        private SqlAccess<TagDistribution> tagDistributions;
        private SqlAccess<Film> films;
        private SqlAccess<CategoryDistribution> categoryDistributions;
        private SqlAccess<DirectorOfFilm> directorOfFilms;
        private SqlAccess<CastOfFilm> castOfFilms;
        private SqlAccess<UserReaction> userReactions;
        private SqlAccess<FilmMetadata> filmMetadata;
        private SqlAccess<FilmMetaLink> filmMetaLinks;
        private SqlAccess<People> people;
        private SqlAccess<Taxonomy> taxonomy;

        public DBContext()
            : base()
        {
            roles = null;
            users = null;
            paymentInfos = null;
            paymentMethods = null;
            tagDistributions = null;
            films = null;
            categoryDistributions = null;
            directorOfFilms = null;
            castOfFilms = null;
            userReactions = null;
            filmMetadata = null;
            filmMetaLinks = null;
            people = null;
            taxonomy = null;
            disposed = false;
        }

        public SqlAccess<Role> Roles { get { return InitSqlAccess<Role>(ref roles); } }
        public SqlAccess<User> Users { get { return InitSqlAccess<User>(ref users); } }
        public SqlAccess<PaymentMethod> PaymentMethods { get { return InitSqlAccess<PaymentMethod>(ref paymentMethods); } }
        public SqlAccess<PaymentInfo> PaymentInfos { get { return InitSqlAccess<PaymentInfo>(ref paymentInfos); } }
        public SqlAccess<TagDistribution> TagDistributions { get { return InitSqlAccess<TagDistribution>(ref tagDistributions); } }
        public SqlAccess<Film> Films { get { return InitSqlAccess<Film>(ref films); } }
        public SqlAccess<CategoryDistribution> CategoryDistributions { get { return InitSqlAccess<CategoryDistribution>(ref categoryDistributions); } }
        public SqlAccess<DirectorOfFilm> DirectorOfFilms { get { return InitSqlAccess<DirectorOfFilm>(ref directorOfFilms); } }
        public SqlAccess<CastOfFilm> CastOfFilms { get { return InitSqlAccess<CastOfFilm>(ref castOfFilms); } }
        public SqlAccess<UserReaction> UserReactions { get { return InitSqlAccess<UserReaction>(ref userReactions); } }
        public SqlAccess<FilmMetadata> FilmMetadata { get { return InitSqlAccess<FilmMetadata>(ref filmMetadata); } }
        public SqlAccess<FilmMetaLink> FilmMetaLinks { get { return InitSqlAccess<FilmMetaLink>(ref filmMetaLinks); } }
        public SqlAccess<People> People { get { return InitSqlAccess<People>(ref people); } }
        public SqlAccess<Taxonomy> Taxonomy { get { return InitSqlAccess<Taxonomy>(ref taxonomy); } }

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
                        DisposeSqlAccess<CategoryDistribution>(ref categoryDistributions);
                        DisposeSqlAccess<TagDistribution>(ref tagDistributions);
                        DisposeSqlAccess<Film>(ref films);
                        DisposeSqlAccess<DirectorOfFilm>(ref directorOfFilms);
                        DisposeSqlAccess<CastOfFilm>(ref castOfFilms);
                        DisposeSqlAccess<UserReaction>(ref userReactions);
                        DisposeSqlAccess<FilmMetadata>(ref filmMetadata);
                        DisposeSqlAccess<People>(ref people);
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
