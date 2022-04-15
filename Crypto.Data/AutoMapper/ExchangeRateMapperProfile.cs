using AutoMapper;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto;

namespace Crypto.Data.AutoMapper;

public class ExchangeRateMapperProfile : Profile
{
    public ExchangeRateMapperProfile()
    {
        CreateMap<ExchangeRate, ExchangeRateDto>()
            .ReverseMap();
    }
}