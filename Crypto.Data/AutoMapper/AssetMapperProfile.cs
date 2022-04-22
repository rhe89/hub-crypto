using AutoMapper;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto;
using Hub.Shared.DataContracts.Crypto.Dto;

namespace Crypto.Data.AutoMapper;

public class AssetMapperProfile : Profile
{
    public AssetMapperProfile()
    {
        CreateMap<Asset, AssetHistoryDto>()
            .ForMember(dest => dest.Value, 
                opt => opt.MapFrom(x => (decimal)x.Value))
            .ReverseMap();
    }
}