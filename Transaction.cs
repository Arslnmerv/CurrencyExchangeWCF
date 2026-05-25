namespace CurrencyExchangeWCF
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Type { get; set; } = ""; // "BUY" or "SELL"
        public string CurrencyCode { get; set; } = "";
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
        public decimal PlnValue { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}