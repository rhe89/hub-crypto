using Hub.Storage.Repository.Dto;

namespace CoinbasePro.Core.Dto.Data
{
    public class AccountBalanceDto : EntityDtoBase
    {
        public long AccountId { get; set; }
        public int Balance { get; set; }
    }
}