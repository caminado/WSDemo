using System.Text.Json.Serialization;

namespace WSDemo.Domain.DTO;

public record FolderItemDto
{
    [JsonIgnore(Condition =JsonIgnoreCondition.WhenWritingNull)]
    public string? Id { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ParentId { get; set; }
    public string Name { get; set; }
}
