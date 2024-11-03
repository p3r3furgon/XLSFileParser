using AutoMapper;
using B1Task2.Models;

namespace B1Task2.UseCases.GetFiles
{
    public class GetFilesRequestMapper : Profile
    {
        public GetFilesRequestMapper()
        {
            CreateMap<AccountSource, FileDto>()
            .ForMember(dest => dest.fileId, opt => opt.MapFrom(c => c.Id))
            .ForMember(dest => dest.fileName, opt => opt.MapFrom(c => c.SourceType))
            .ForMember(dest => dest.dateAdded, opt => opt.MapFrom(c => c.UploadDate));
        }
    }
}
