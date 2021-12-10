using System;
using AutoMapper;
using CoinbasePro.Data.Entities;
using Hub.Shared.DataContracts.Banking;

namespace CoinbasePro.Data.AutoMapper
{
    public class AccountBalanceMapperProfile : Profile
    {
        public AccountBalanceMapperProfile()
        {
            CreateMap<AccountBalance, AccountBalanceDto>()
                .ForMember(dest => dest.Balance, 
                    opt => opt.MapFrom(x => (decimal)x.Value))
                .ReverseMap();
        }
    }
}