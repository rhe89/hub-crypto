using AutoMapper;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto.Dto;

namespace Crypto.Data.AutoMapper;

public class CurrencyMapperProfile : Profile
{
    public CurrencyMapperProfile()
    {
        CreateMap<Currency, CurrencyDto>()
            .ReverseMap();
    }
}