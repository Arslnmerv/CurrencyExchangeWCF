using CoreWCF;

namespace CurrencyExchangeWCF
{
    [ServiceContract]
    public interface IExchangeService
    {
        [OperationContract]
        string GetMessage();
    }

    public class ExchangeService : IExchangeService
    {
        public string GetMessage()
        {
            return "Hello from WCF Service!";
        }
    }
}