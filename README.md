\# CurrencyExchangeWCF



A network-based currency exchange office system developed as part of the Network Application Development course.



\## About



This project simulates an online currency exchange office using WCF (Windows Communication Foundation) on the .NET platform. It retrieves live exchange rates from the National Bank of Poland (NBP) API.



\## Structure



\- \*\*CurrencyExchangeWCF\*\* — WCF web service containing the business logic

\- \*\*CurrencyExchangeClient\*\* — Console application that communicates with the service



\## How to Run



Start the service:

```bash

dotnet run

```



Then in a separate terminal, run the client:

```bash

cd CurrencyExchangeClient

dotnet run

```



\## Technologies

\- .NET 10

\- CoreWCF

\- NBP Public API

