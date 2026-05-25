using System.ComponentModel.DataAnnotations;

namespace CurrencyExchangeWCF
{
    public class UserAccount
    {
        [Key]
        public string Username { get; set; } = "";
        public decimal PlnBalance { get; set; } = 0;
        public string CurrencyBalancesJson { get; set; } = "{}";
    }
}