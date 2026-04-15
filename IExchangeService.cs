using CoreWCF;

namespace CurrencyExchangeWCF
{
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
}