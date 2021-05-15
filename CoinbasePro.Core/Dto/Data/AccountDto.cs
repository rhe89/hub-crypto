using System.Collections.Generic;
using Hub.Storage.Repository.Dto;

namespace CoinbasePro.Core.Dto.Data
{
    public class AccountDto : EntityDtoBase
    {
        public string Currency { get; set; }

        public ICollection<AssetDto> Assets { get; set; }
    }
}
