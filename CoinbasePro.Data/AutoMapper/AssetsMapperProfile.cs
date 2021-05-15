using AutoMapper;
using CoinbasePro.Core.Dto.Data;
using CoinbasePro.Core.Entities;

namespace CoinbasePro.Data.AutoMapper
{
    public class AssetsMapperProfile : Profile
    {
        public AssetsMapperProfile()
        {
            CreateMap<Asset, AssetDto>()
                .ReverseMap();
        }
    }
}