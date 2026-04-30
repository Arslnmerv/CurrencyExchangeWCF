# CurrencyExchangeWCF

A network-based currency exchange office system developed as part of the Network Application Development course.

## About

This project simulates an online currency exchange office using WCF (Windows Communication Foundation) on the .NET platform. It retrieves live exchange rates from the National Bank of Poland (NBP) API.

## Structure

- **CurrencyExchangeWCF** — WCF web service containing the business logic
- **CurrencyExchangeClient** — Console application that communicates with the service
- **CurrencyExchangeWPF** — WPF desktop client application with a graphical user interface

## How to Run

Start the service:

```bash
dotnet run
```

Then run the WPF client or console client in a separate terminal:

```bash
cd CurrencyExchangeWPF
dotnet run
```

Or the console client:

```bash
cd CurrencyExchangeClient
dotnet run
```

## Features

- Live exchange rates from the NBP API
- Buy and sell currencies with 2% spread
- User account and PLN balance management
- WPF graphical interface with real-time log

## Technologies

- .NET 10
- CoreWCF
- NBP Public API
- WPF (Windows Presentation Foundation)

## Labs Completed

| Lab | Description | Status |
|-----|-------------|--------|
| Lab 1 | WCF service + console client | ✅ |
| Lab 2-4 | NBP API integration | ✅ |
| Lab 5-6 | Exchange office architecture and currency exchange logic | ✅ |
| Lab 7 | NBP API integration (refined) | ✅ |
| Lab 8 | WPF client application | ✅ |