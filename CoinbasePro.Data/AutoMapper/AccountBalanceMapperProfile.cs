using AutoMapper;
using CoinbasePro.Core.Dto.Data;
using CoinbasePro.Core.Entities;

namespace CoinbasePro.Data.AutoMapper
{
    public class AccountBalanceMapperProfile : Profile
    {
        public AccountBalanceMapperProfile()
        {
            CreateMap<AccountBalance, AccountBalanceDto>()
                .ForMember(dest => dest.Balance, 
                    opt => opt.MapFrom(x => x.Value))
                .ReverseMap();
        }
    }
}