using AutoMapper;
using CoinbasePro.Data.Entities;
using Hub.Shared.DataContracts.Banking;

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