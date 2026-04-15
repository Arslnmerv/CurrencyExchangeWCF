using System.Text.Json;

namespace CurrencyExchangeWCF
{
    public class NbpApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://api.nbp.pl/api";

        public NbpApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<decimal> GetRateAsync(string currencyCode)
        {
            var url = $"{BaseUrl}/exchangerates/rates/A/{currencyCode.ToUpper()}/?format=json";
            var response = await _httpClient.GetStringAsync(url);

            var doc = JsonDocument.Parse(response);
            var rate = doc.RootElement
                .GetProperty("rates")[0]
                .GetProperty("mid")
                .GetDecimal();

            return rate;
        }

        public async Task<List<string>> GetAvailableCurrenciesAsync()
        {
            var url = $"{BaseUrl}/exchangerates/tables/A/?format=json";
            var response = await _httpClient.GetStringAsync(url);

            var doc = JsonDocument.Parse(response);
            var rates = doc.RootElement[0].GetProperty("rates");

            var currencies = new List<string>();
            foreach (var rate in rates.EnumerateArray())
            {
                currencies.Add(rate.GetProperty("code").GetString()!);
            }

            return currencies;
        }
    }
}