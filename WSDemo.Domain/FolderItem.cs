using Microsoft.EntityFrameworkCore;

namespace WSDemo.Domain;

[Index(nameof(ParentId))]
public class FolderItem : BaseEntity
{
    public Guid? ParentId { get; set; }
    public string Name { get; set; }
}
