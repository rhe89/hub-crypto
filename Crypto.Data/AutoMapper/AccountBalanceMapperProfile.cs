using AutoMapper;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto.Dto;

namespace Crypto.Data.AutoMapper;

public class AccountBalanceMapperProfile : Profile
{
    public AccountBalanceMapperProfile()
    {
        CreateMap<AccountBalance, AccountBalanceDto>()
            .ReverseMap()
            .ForMember(x => x.Account, opt => opt.Ignore());
    }
}