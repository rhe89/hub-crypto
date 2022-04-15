using AutoMapper;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto;

namespace Crypto.Data.AutoMapper;

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