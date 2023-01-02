using AutoMapper;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto.Dto;

namespace Crypto.Data.AutoMapper;

public class CurrencyPriceMapperProfile : Profile
{
    public CurrencyPriceMapperProfile()
    {
        CreateMap<CurrencyPrice, CurrencyPriceDto>()
            .ReverseMap()
            .ForMember(x => x.Currency, opt => opt.Ignore());
    }
}