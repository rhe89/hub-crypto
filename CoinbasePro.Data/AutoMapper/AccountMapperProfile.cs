using AutoMapper;
using CoinbasePro.Core.Dto.Data;
using CoinbasePro.Core.Entities;

namespace CoinbasePro.Data.AutoMapper
{
    public class AccountMapperProfile : Profile
    {
        public AccountMapperProfile()
        {
            CreateMap<Account, AccountDto>()
                .ForMember(dest => dest.Balance, 
                    opt => opt.MapFrom(x => x.CurrentBalance))
                .ForMember(dest => dest.Name, 
                    opt => opt.MapFrom(x => x.Currency))
                .ReverseMap();
        }
    }
}