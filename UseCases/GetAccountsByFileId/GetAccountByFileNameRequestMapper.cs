using AutoMapper;
using B1Task2.Models;

namespace B1Task2.UseCases.GetAccountsByFileId
{
    public class GetAccountByFileNameRequestMapper : Profile
    {
        public GetAccountByFileNameRequestMapper()
        {
            CreateMap<Account, AccountInfoDto>()
            .ForMember(dest => dest.AccountCode, opt => opt.MapFrom(c => c.AccountCode))
            .ForMember(dest => dest.ClassCode, opt => opt.MapFrom(c => c.Class.ClassCode))
            .ForMember(dest => dest.ClassName, opt => opt.MapFrom(c => c.Class.ClassName));
        }
    }
}
