using System.ServiceModel;

[ServiceContract]
public interface IExchangeService
{
    [OperationContract] Task<decimal> GetExchangeRate(string currencyCode);
    [OperationContract] Task<decimal> BuyCurrency(string currencyCode, decimal amount);
    [OperationContract] Task<decimal> SellCurrency(string currencyCode, decimal amount);
    [OperationContract] Task<List<string>> GetAvailableCurrencies();
    [OperationContract] Task<bool> CreateAccount(string username);
    [OperationContract] Task<bool> Deposit(string username, decimal amount);
    [OperationContract] Task<decimal> GetPlnBalance(string username);
    [OperationContract] Task<decimal> GetCurrencyBalance(string username, string currencyCode);
    [OperationContract] Task<bool> BuyCurrencyForUser(string username, string currencyCode, decimal amount);
    [OperationContract] Task<bool> SellCurrencyForUser(string username, string currencyCode, decimal amount);
}

class Program
{
    static async Task Main(string[] args)
    {
        var binding = new BasicHttpBinding();
        var endpoint = new EndpointAddress("http://localhost:58749/ExchangeService.svc");
        var channelFactory = new ChannelFactory<IExchangeService>(binding, endpoint);
        var client = channelFactory.CreateChannel();

        // Create account and deposit
        await client.CreateAccount("testuser");
        await client.Deposit("testuser", 1000);

        // Check PLN balance
        var balance = await client.GetPlnBalance("testuser");
        Console.WriteLine($"PLN Balance: {balance}");

        // Get USD rate
        var rate = await client.GetExchangeRate("USD");
        Console.WriteLine($"USD Rate: {rate}");

        // Buy 10 USD
        var bought = await client.BuyCurrencyForUser("testuser", "USD", 10);
        Console.WriteLine($"Bought 10 USD: {bought}");

        // Check balances
        var plnAfter = await client.GetPlnBalance("testuser");
        var usdBalance = await client.GetCurrencyBalance("testuser", "USD");
        Console.WriteLine($"PLN after purchase: {plnAfter}");
        Console.WriteLine($"USD Balance: {usdBalance}");

        await ((IAsyncDisposable)client).DisposeAsync();
    }
}