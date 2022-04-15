using AutoMapper;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto;

namespace Crypto.Data.AutoMapper;

public class AccountBalanceMapperProfile : Profile
{
    public AccountBalanceMapperProfile()
    {
        CreateMap<AccountBalance, AccountBalanceDto>()
            .ForMember(dest => dest.Balance, 
                opt => opt.MapFrom(x => (decimal)x.Value))
            .ReverseMap();
    }
}