using Hub.Storage.Repository.Dto;

namespace CoinbasePro.Core.Dto.Data
{
    public class AssetDto : EntityDtoBase
    {
        public long AccountId { get; set; }
        public int Value { get; set; }

        public AccountDto Account { get; set; }  
    }
}