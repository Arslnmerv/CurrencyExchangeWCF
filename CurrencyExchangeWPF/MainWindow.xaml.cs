using System.ServiceModel;
using System.Windows;

namespace CurrencyExchangeWPF
{
    [ServiceContract]
    public interface IExchangeService
    {
        [OperationContract] System.Threading.Tasks.Task<decimal> GetExchangeRate(string currencyCode);
        [OperationContract] System.Threading.Tasks.Task<bool> CreateAccount(string username);
        [OperationContract] System.Threading.Tasks.Task<bool> Deposit(string username, decimal amount);
        [OperationContract] System.Threading.Tasks.Task<decimal> GetPlnBalance(string username);
        [OperationContract] System.Threading.Tasks.Task<decimal> GetCurrencyBalance(string username, string currencyCode);
        [OperationContract] System.Threading.Tasks.Task<bool> BuyCurrencyForUser(string username, string currencyCode, decimal amount);
        [OperationContract] System.Threading.Tasks.Task<bool> SellCurrencyForUser(string username, string currencyCode, decimal amount);
    }

    public partial class MainWindow : Window
    {
        private IExchangeService _client;
        private string _username = "";

        public MainWindow()
        {
            InitializeComponent();
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress("http://localhost:58749/ExchangeService.svc");
            var factory = new ChannelFactory<IExchangeService>(binding, endpoint);
            _client = factory.CreateChannel();
        }

        private void Log(string message)
        {
            LogBox.Items.Insert(0, $"{DateTime.Now:HH:mm:ss} — {message}");
        }

        private async void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            _username = UsernameBox.Text.Trim();
            if (string.IsNullOrEmpty(_username)) return;
            var result = await _client.CreateAccount(_username);
            Log(result ? $"Account '{_username}' created." : $"Account '{_username}' already exists.");
        }

        private async void LoadBalance_Click(object sender, RoutedEventArgs e)
        {
            _username = UsernameBox.Text.Trim();
            if (string.IsNullOrEmpty(_username)) return;
            var balance = await _client.GetPlnBalance(_username);
            BalanceText.Text = $"PLN Balance: {balance:F2}";
        }

        private async void Deposit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_username)) { Log("No account selected."); return; }
            if (!decimal.TryParse(DepositBox.Text, out var amount)) { Log("Invalid amount."); return; }
            var result = await _client.Deposit(_username, amount);
            if (result)
            {
                Log($"Deposited {amount:F2} PLN.");
                var balance = await _client.GetPlnBalance(_username);
                BalanceText.Text = $"PLN Balance: {balance:F2}";
            }
        }

        private async void GetRate_Click(object sender, RoutedEventArgs e)
        {
            var currency = CurrencyBox.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(currency)) return;
            var rate = await _client.GetExchangeRate(currency);
            RateText.Text = $"1 {currency} = {rate:F4} PLN";
            Log($"Rate for {currency}: {rate:F4} PLN");
        }

        private async void Buy_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_username)) { Log("No account selected."); return; }
            var currency = BuyCurrencyBox.Text.Trim().ToUpper();
            if (!decimal.TryParse(BuyAmountBox.Text, out var amount)) { Log("Invalid amount."); return; }
            var result = await _client.BuyCurrencyForUser(_username, currency, amount);
            if (result)
            {
                Log($"Bought {amount} {currency}.");
                var balance = await _client.GetPlnBalance(_username);
                BalanceText.Text = $"PLN Balance: {balance:F2}";
            }
            else Log("Purchase failed. Check balance.");
        }

        private async void Sell_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_username)) { Log("No account selected."); return; }
            var currency = SellCurrencyBox.Text.Trim().ToUpper();
            if (!decimal.TryParse(SellAmountBox.Text, out var amount)) { Log("Invalid amount."); return; }
            var result = await _client.SellCurrencyForUser(_username, currency, amount);
            if (result)
            {
                Log($"Sold {amount} {currency}.");
                var balance = await _client.GetPlnBalance(_username);
                BalanceText.Text = $"PLN Balance: {balance:F2}";
            }
            else Log("Sale failed. Check currency balance.");
        }
    }
}