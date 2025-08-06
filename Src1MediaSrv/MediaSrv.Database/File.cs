using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaSrv.Database;

[Table("Files")]
public class File
{
    [Key]
    [Column("Id", TypeName = "varchar(100)")]
    public string Id { get; set; } = default!;

    [Column("PartitionKey", TypeName = "varchar(100)")]
    public string PartitionKey { get; set; } = default!;

    [Column("FileName", TypeName = "varchar(255)")]
    public string FileName { get; set; } = default!;

    [Column("Type", TypeName = "varchar(20)")]
    public string Type { get; set; } = default!;

    [Column("Title", TypeName = "nvarchar(255)")]
    public string Title { get; set; } = default!;

    [Column("Description", TypeName = "nvarchar(1000)")]
    public string? Description { get; set; }
}
