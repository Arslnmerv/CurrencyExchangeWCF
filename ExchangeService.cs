namespace CurrencyExchangeWCF
{
    public class ExchangeService : IExchangeService
    {
        private readonly NbpApiClient _nbpClient = new();

        private const decimal BuySpread = 0.02m;
        private const decimal SellSpread = 0.02m;

        public async Task<decimal> GetExchangeRate(string currencyCode)
        {
            return await _nbpClient.GetRateAsync(currencyCode);
        }

        public async Task<decimal> BuyCurrency(string currencyCode, decimal amount)
        {
            var midRate = await _nbpClient.GetRateAsync(currencyCode);
            var buyRate = midRate * (1 - BuySpread);
            return amount * buyRate;
        }

        public async Task<decimal> SellCurrency(string currencyCode, decimal amount)
        {
            var midRate = await _nbpClient.GetRateAsync(currencyCode);
            var sellRate = midRate * (1 + SellSpread);
            return amount * sellRate;
        }

        public async Task<List<string>> GetAvailableCurrencies()
        {
            return await _nbpClient.GetAvailableCurrenciesAsync();
        }
    }
}