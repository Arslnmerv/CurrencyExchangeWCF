namespace CurrencyExchangeWCF
{
    public class UserAccount
    {
        public string Username { get; set; } = "";
        public decimal PlnBalance { get; set; } = 0;
        public Dictionary<string, decimal> CurrencyBalances { get; set; } = new();
    }
}