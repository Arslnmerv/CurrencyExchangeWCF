using System.ServiceModel;

[ServiceContract]
public interface IExchangeService
{
    [OperationContract]
    Task<decimal> GetExchangeRate(string currencyCode);

    [OperationContract]
    Task<decimal> BuyCurrency(string currencyCode, decimal amount);

    [OperationContract]
    Task<decimal> SellCurrency(string currencyCode, decimal amount);

    [OperationContract]
    Task<List<string>> GetAvailableCurrencies();
}

class Program
{
    static async Task Main(string[] args)
    {
        var binding = new BasicHttpBinding();
        var endpoint = new EndpointAddress("http://localhost:58749/ExchangeService.svc");

        var channelFactory = new ChannelFactory<IExchangeService>(binding, endpoint);
        var client = channelFactory.CreateChannel();

        var rate = await client.GetExchangeRate("USD");
        Console.WriteLine($"USD rate: {rate}");

        await ((IAsyncDisposable)client).DisposeAsync();
    }
}