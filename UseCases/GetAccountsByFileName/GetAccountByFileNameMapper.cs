using AutoMapper;
using B1Task2.Models;

namespace B1Task2.UseCases.GetAccountsByFileName
{
    public class GetAccountByFileNameMapper : Profile
    {
        public GetAccountByFileNameMapper()
        {
            CreateMap<Account, AccountInfoDto>()
            .ForMember(dest => dest.AccountCode, opt => opt.MapFrom(c => c.AccountCode))
            .ForMember(dest => dest.ClassCode, opt => opt.MapFrom(c => c.Class.ClassCode))
            .ForMember(dest => dest.ClassName, opt => opt.MapFrom(c => c.Class.ClassName));
        }
    }
}
