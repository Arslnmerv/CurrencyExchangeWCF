using CoreWCF;
using CoreWCF.Configuration;
using CurrencyExchangeWCF;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<ExchangeService>();

var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<ExchangeService>();
    serviceBuilder.AddServiceEndpoint<ExchangeService, IExchangeService>(
        new BasicHttpBinding(),
        "/ExchangeService.svc"
    );
});

var serviceMetadataBehavior = app.Services
    .GetRequiredService<CoreWCF.Description.ServiceMetadataBehavior>();
serviceMetadataBehavior.HttpGetEnabled = true;

app.Run();