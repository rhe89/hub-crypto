using AutoMapper;
using Crypto.Data.Entities;
using Hub.Shared.DataContracts.Crypto.Dto;

namespace Crypto.Data.AutoMapper;

public class WalletMapperProfile : Profile
{
    public WalletMapperProfile()
    {
        CreateMap<Wallet, WalletDto>()
            .ReverseMap();
    }
}