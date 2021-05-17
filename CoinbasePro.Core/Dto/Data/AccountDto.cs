using Hub.Storage.Repository.Dto;

namespace CoinbasePro.Core.Dto.Data
{
    public class AccountDto : EntityDtoBase
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
    }
}
