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
                .ReverseMap();
        }
    }
}