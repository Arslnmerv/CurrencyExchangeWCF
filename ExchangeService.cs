namespace CurrencyExchangeWCF
{
    public class ExchangeService : IExchangeService
    {
        private readonly NbpApiClient _nbpClient = new();
        private const decimal BuySpread = 0.02m;
        private const decimal SellSpread = 0.02m;

        private static readonly Dictionary<string, UserAccount> _accounts = new();

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

        public Task<bool> CreateAccount(string username)
        {
            if (_accounts.ContainsKey(username))
                return Task.FromResult(false);

            _accounts[username] = new UserAccount { Username = username };
            return Task.FromResult(true);
        }

        public Task<bool> Deposit(string username, decimal amount)
        {
            if (!_accounts.TryGetValue(username, out var account))
                return Task.FromResult(false);

            account.PlnBalance += amount;
            return Task.FromResult(true);
        }

        public Task<decimal> GetPlnBalance(string username)
        {
            if (!_accounts.TryGetValue(username, out var account))
                return Task.FromResult(0m);

            return Task.FromResult(account.PlnBalance);
        }

        public Task<decimal> GetCurrencyBalance(string username, string currencyCode)
        {
            if (!_accounts.TryGetValue(username, out var account))
                return Task.FromResult(0m);

            account.CurrencyBalances.TryGetValue(currencyCode, out var balance);
            return Task.FromResult(balance);
        }

        public async Task<bool> BuyCurrencyForUser(string username, string currencyCode, decimal amount)
        {
            if (!_accounts.TryGetValue(username, out var account))
                return false;

            var midRate = await _nbpClient.GetRateAsync(currencyCode);
            var sellRate = midRate * (1 + SellSpread);
            var totalCost = amount * sellRate;

            if (account.PlnBalance < totalCost)
                return false;

            account.PlnBalance -= totalCost;
            account.CurrencyBalances.TryGetValue(currencyCode, out var current);
            account.CurrencyBalances[currencyCode] = current + amount;
            return true;
        }

        public async Task<bool> SellCurrencyForUser(string username, string currencyCode, decimal amount)
        {
            if (!_accounts.TryGetValue(username, out var account))
                return false;

            account.CurrencyBalances.TryGetValue(currencyCode, out var currentBalance);
            if (currentBalance < amount)
                return false;

            var midRate = await _nbpClient.GetRateAsync(currencyCode);
            var buyRate = midRate * (1 - BuySpread);

            account.CurrencyBalances[currencyCode] = currentBalance - amount;
            account.PlnBalance += amount * buyRate;
            return true;
        }
    }
}