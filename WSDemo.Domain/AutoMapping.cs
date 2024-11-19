using AutoMapper;
using WSDemo.Domain.DTO;

namespace WSDemo.Domain;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<FolderItem, FolderItemDto>();
        CreateMap<FolderItemDto, FolderItem>()
            .ForMember(d => d.Id,
                opt => opt.MapFrom(s => string.IsNullOrWhiteSpace(s.Id) ? Guid.NewGuid().ToString() : s.Id))
            .ForMember(x => x.ParentId,
                opt => opt.MapFrom(s => string.IsNullOrWhiteSpace(s.ParentId) ? null : s.ParentId)
        );
    }
}
