using AutoMapper;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto.Dto;

namespace Crypto.Data.AutoMapper;

public class AccountMapperProfile : Profile
{
    public AccountMapperProfile()
    {
        CreateMap<Account, AccountDto>()
            .ReverseMap()
            .ForMember(x => x.Currency, opt => opt.Ignore())
            .ForMember(x => x.Wallet, opt => opt.Ignore());
    }
}