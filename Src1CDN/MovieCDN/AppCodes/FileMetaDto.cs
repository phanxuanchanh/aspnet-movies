namespace MovieCDN.AppCodes;

public class FileMetaDto
{
    public string Id { get; set; } = default!;
    public string PartitionKey { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
}

public class FileMetaInput
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
}