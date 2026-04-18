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

        [OperationContract]
        Task<bool> CreateAccount(string username);

        [OperationContract]
        Task<bool> Deposit(string username, decimal amount);

        [OperationContract]
        Task<decimal> GetPlnBalance(string username);

        [OperationContract]
        Task<decimal> GetCurrencyBalance(string username, string currencyCode);

        [OperationContract]
        Task<bool> BuyCurrencyForUser(string username, string currencyCode, decimal amount);

        [OperationContract]
        Task<bool> SellCurrencyForUser(string username, string currencyCode, decimal amount);
    }
}