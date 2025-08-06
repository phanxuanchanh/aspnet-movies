using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaSrv.Database;

[Table("ApiKeys")]
public class ApiKey
{
    [Column("ClientId", TypeName = "nvarchar(200)")]
    [Key]
    public string ClientId { get; set; } = string.Empty;

    [Column("SecretKey", TypeName = "nvarchar(500)")]
    public string SecretKey { get; set; } = string.Empty;
}