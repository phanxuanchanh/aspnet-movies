using MSSQL;
using MSSQL.Attributes;

namespace Data.DAL
{
    [SqlTable("User")]
    public class User : SqlTableWithTimestamp
    {
        [SqlColumn("ID", PrimaryKey = true)]
        public string Id { get; set; }

        [SqlColumn("UserName")]
        public string UserName { get; set; }

        [SqlColumn("SurName")]
        public string SurName { get; set; }

        [SqlColumn("MiddleName")]
        public string MiddleName { get; set; }

        [SqlColumn("Name")]
        public string Name { get; set; }

        [SqlColumn("Email")]
        public string Email { get; set; }

        [SqlColumn("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [SqlColumn("Password")]
        public string Password { get; set; }

        [SqlColumn("Salt")]
        public string Salt { get; set; }

        [SqlColumn("Description")]
        public string Description { get; set; }

        [SqlColumn("Activated")]
        public bool Activated { get; set; }

        [SqlColumn("RoleId")]
        public string RoleId { get; set; }
    }
}
