using AutoMapper;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto.Dto;

namespace Crypto.Data.AutoMapper;

public class ExchangeRateMapperProfile : Profile
{
    public ExchangeRateMapperProfile()
    {
        CreateMap<ExchangeRate, ExchangeRateDto>()
            .ReverseMap();
    }
}