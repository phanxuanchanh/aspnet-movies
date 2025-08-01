using MSSQL;
using MSSQL.Attributes;

namespace Data.DAL
{
    [SqlTable("User")]
    public class User : SqlTableWithTimestamp
    {
        [SqlColumn("ID", PrimaryKey = true)]
        public string Id { get; set; }

        [SqlColumn("userName")]
        public string UserName { get; set; }

        [SqlColumn("surName")]
        public string SurName { get; set; }

        [SqlColumn("middleName")]
        public string MiddleName { get; set; }

        [SqlColumn("name")]
        public string Name { get; set; }

        [SqlColumn("email")]
        public string Email { get; set; }

        [SqlColumn("phoneNumber")]
        public string PhoneNumber { get; set; }

        [SqlColumn("password")]
        public string Password { get; set; }

        [SqlColumn("salt")]
        public string Salt { get; set; }

        [SqlColumn("description")]
        public string Description { get; set; }

        [SqlColumn("activated")]
        public bool Activated { get; set; }

        [SqlColumn("roleId")]
        public string RoleId { get; set; }
    }
}
