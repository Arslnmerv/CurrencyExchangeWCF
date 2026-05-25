using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchangeWCF
{
    public class ExchangeService : IExchangeService
    {
        private readonly NbpApiClient _nbpClient = new();
        private const decimal BuySpread = 0.02m;
        private const decimal SellSpread = 0.02m;

        private Dictionary<string, decimal> GetBalances(UserAccount account)
        {
            return JsonSerializer.Deserialize<Dictionary<string, decimal>>(account.CurrencyBalancesJson) ?? new();
        }

        private void SetBalances(UserAccount account, Dictionary<string, decimal> balances)
        {
            account.CurrencyBalancesJson = JsonSerializer.Serialize(balances);
        }

        public async Task<decimal> GetExchangeRate(string currencyCode)
        {
            return await _nbpClient.GetRateAsync(currencyCode);
        }

        public async Task<decimal> BuyCurrency(string currencyCode, decimal amount)
        {
            var midRate = await _nbpClient.GetRateAsync(currencyCode);
            return amount * midRate * (1 - BuySpread);
        }

        public async Task<decimal> SellCurrency(string currencyCode, decimal amount)
        {
            var midRate = await _nbpClient.GetRateAsync(currencyCode);
            return amount * midRate * (1 + SellSpread);
        }

        public async Task<List<string>> GetAvailableCurrencies()
        {
            return await _nbpClient.GetAvailableCurrenciesAsync();
        }

        public async Task<bool> CreateAccount(string username)
        {
            using var db = new AppDbContext();
            await db.Database.EnsureCreatedAsync();
            if (await db.Users.AnyAsync(u => u.Username == username))
                return false;
            db.Users.Add(new UserAccount { Username = username });
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deposit(string username, decimal amount)
        {
            using var db = new AppDbContext();
            var account = await db.Users.FindAsync(username);
            if (account == null) return false;
            account.PlnBalance += amount;
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetPlnBalance(string username)
        {
            using var db = new AppDbContext();
            var account = await db.Users.FindAsync(username);
            return account?.PlnBalance ?? 0;
        }

        public async Task<decimal> GetCurrencyBalance(string username, string currencyCode)
        {
            using var db = new AppDbContext();
            var account = await db.Users.FindAsync(username);
            if (account == null) return 0;
            var balances = GetBalances(account);
            balances.TryGetValue(currencyCode, out var balance);
            return balance;
        }

        public async Task<bool> BuyCurrencyForUser(string username, string currencyCode, decimal amount)
        {
            using var db = new AppDbContext();
            var account = await db.Users.FindAsync(username);
            if (account == null) return false;

            var midRate = await _nbpClient.GetRateAsync(currencyCode);
            var sellRate = midRate * (1 + SellSpread);
            var totalCost = amount * sellRate;

            if (account.PlnBalance < totalCost) return false;

            account.PlnBalance -= totalCost;
            var balances = GetBalances(account);
            balances.TryGetValue(currencyCode, out var current);
            balances[currencyCode] = current + amount;
            SetBalances(account, balances);

            db.Transactions.Add(new Transaction
            {
                Username = username,
                Type = "BUY",
                CurrencyCode = currencyCode,
                Amount = amount,
                Rate = sellRate,
                PlnValue = totalCost
            });

            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SellCurrencyForUser(string username, string currencyCode, decimal amount)
        {
            using var db = new AppDbContext();
            var account = await db.Users.FindAsync(username);
            if (account == null) return false;

            var balances = GetBalances(account);
            balances.TryGetValue(currencyCode, out var currentBalance);
            if (currentBalance < amount) return false;

            var midRate = await _nbpClient.GetRateAsync(currencyCode);
            var buyRate = midRate * (1 - BuySpread);
            var plnReceived = amount * buyRate;

            balances[currencyCode] = currentBalance - amount;
            SetBalances(account, balances);
            account.PlnBalance += plnReceived;

            db.Transactions.Add(new Transaction
            {
                Username = username,
                Type = "SELL",
                CurrencyCode = currencyCode,
                Amount = amount,
                Rate = buyRate,
                PlnValue = plnReceived
            });

            await db.SaveChangesAsync();
            return true;
        }
    }
}