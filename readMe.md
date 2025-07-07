# Stock Quote Alert Service

This project is a console application developed in C\# that monitors the quotation of a B3 (Brazilian Stock Exchange) asset and sends email alerts if its price falls below a specified purchase point or rises above a sale point.

## Table of Contents

  * [Features](https://www.google.com/search?q=%23features)
  * [Technologies Used](https://www.google.com/search?q=%23technologies-used)
  * [Getting Started](https://www.google.com/search?q=%23getting-started)
      * [Prerequisites](https://www.google.com/search?q=%23prerequisites)
      * [Installation](https://www.google.com/search?q=%23installation)
      * [Configuration](https://www.google.com/search?q=%23configuration)
      * [Running the Application](https://www.google.com/search?q=%23running-the-application)
  * [Project Structure](https://www.google.com/search?q=%23project-structure)
  * [Design Choices and Best Practices](https://www.google.com/search?q=%23design-choices-and-best-practices)
  * [Extra Features](https://www.google.com/search?q=%23extra-features)
  * [Future Improvements](https://www.google.com/search?q=%23future-improvements)
  * [License](https://www.google.com/search?q=%23license)
  * [Contact](https://www.google.com/search?q=%23contact)

## Features

  * [cite\_start]Continuously monitors a specified B3 stock ticker. [cite: 1, 35]
  * [cite\_start]Sends email notifications when the stock price drops to or below a defined purchase point. [cite: 1, 23, 51]
  * [cite\_start]Sends email notifications when the stock price rises to or above a defined sale point. [cite: 1, 23, 50]
  * [cite\_start]Configurable via `AppSettings.json` for email destination and SMTP server settings. [cite: 1, 31, 32, 33]
  * Input validation for command-line arguments.
  * [cite\_start]Confirmation email sent on setup to verify email functionality. [cite: 74]

## Technologies Used

  * [cite\_start]**C\#**: The primary programming language used. [cite: 1, 12]
  * **.NET**: Framework for building the console application.
  * [cite\_start]**Brapi API**: Chosen for fetching stock quotations due to its free tier and B3 coverage. [cite: 63]
  * [cite\_start]**`IHttpClientFactory`**: Used for efficient management and reuse of HTTP connections, preventing `SocketException`. [cite: 70]
  * [cite\_start]**SMTP Client**: For sending email notifications. [cite: 77]
  * **`Microsoft.Extensions.Configuration`**: For handling application settings from `AppSettings.json`.
  * **`Microsoft.Extensions.Hosting`**: To support dependency injection and host management.

## Getting Started

Follow these instructions to get a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

  * .NET SDK (compatible with .NET 6 or later)
  * A Brapi API key (a free tier is available)
  * An email account configured for SMTP sending (e.g., Gmail with app passwords enabled)

### Installation

1.  **Clone the repository:**

    ```bash
    git clone https://github.com/GbrLomenha/stock-quote.git
    cd stock-quote
    ```

2.  **Restore NuGet packages:**

    ```bash
    dotnet restore
    ```

### Configuration

Before running the application, you need to configure the `AppSettings.json` file with your API key and email settings.

Create an `AppSettings.json` file in the root directory of the project with the following structure:

```json
{
    "ApiKey": "BRAPI_API_KEY",
    "EmailConfig": {
        "SmtpServer": "smtp.your-email-provider.com",
        "SmtpPort": 587,
        "SmtpUser": "your-email@example.com",
        "SmtpPassword": "your-email-password",
        "From": "your-email@example.com",
        "To": "recipient-email@example.com"
    },
    "VerificationInterval": "30"
}

```

  * `ApiKey`: Your API key from Brapi.
  * `EmailConfig`:
      * `SmtpServer`: Your SMTP server address (e.g., `smtp.gmail.com` for Gmail).
      * `SmtpPort`: Your SMTP server port (e.g., `587` for TLS/STARTTLS).
      * `SmtpUser`: The email address used to send alerts.
      * `SmtpPassword`: The password for the sending email account. If using Gmail, you might need to generate an [App Password](https://support.google.com/accounts/answer/185833?hl=en).
      * `From`: The "From" email address that will appear in the alerts.
      * [cite\_start]`To`: The destination email address for the alerts. [cite: 1, 32]
  * `VerificationInterval`: The interval in minutes between stock quotation checks. [cite\_start]For the free Brapi plan, a minimum of `30` minutes is recommended due to data update frequency. [cite: 73]

### Running the Application

[cite\_start]The application is a console application that takes three command-line arguments: the stock ticker symbol, the purchase price point, and the sale price point. [cite: 1, 25]

```bash
dotnet run <SYMBOL> <PURCHASE_POINT> <SALE_POINT>
```

**Example:**

```bash
dotnet run PETR4 22.67 22.59
```

[cite\_start]This command will start monitoring `PETR4` and send purchase alerts if the price drops to or below R$22.59, and sale alerts if the price rises to or above R$22.67. [cite: 1, 30]

## Project Structure

  * **`Program.cs`**: The entry point of the application, responsible for setting up the host, configuration, dependency injection, and initiating the monitoring process.
  * **`Quotation.Models`**: Contains data models for API responses (`ApiResponse`, `ApiRootResponse`), stock quotations (`StockQuotation`), and email configuration (`EmailConfig`).
  * **`Quotation.Services`**: Contains the core logic of the application:
      * **`StockQuoteService.cs`**: Handles fetching stock quotations from the Brapi API and monitoring price points.
      * **`EmailService.cs`**: Manages sending email notifications, including purchase, sale, and confirmation emails.
      * **`InputTreatment.cs`**: Static class for validating and sanitizing command-line input arguments.

## Design Choices and Best Practices

  * **Dependency Injection**: Utilizes `.NET`'s built-in dependency injection for `IHttpClientFactory`, `StockQuoteService`, and `EmailService` to promote modularity and testability.
  * [cite\_start]**`IHttpClientFactory`**: Employed to manage `HttpClient` instances, preventing common issues like socket exhaustion and improving resource management. [cite: 70]
  * [cite\_start]**Separation of Concerns**: Email sending logic is encapsulated in `EmailService` to allow for reuse and maintainability, separate from stock monitoring. [cite: 78, 79]
  * [cite\_start]**Decimal for Financial Calculations**: `decimal` type is used for all monetary calculations to ensure high precision and avoid rounding errors common with `float` or `double`. [cite: 66]
  * **Configuration Management**: Application settings are loaded from `AppSettings.json` using `IConfiguration`, allowing for easy modification without recompilation.
  * **Error Handling**: Includes basic error handling for API requests and email sending, with informative console output.
  * [cite\_start]**Continuous Monitoring**: The program runs in a continuous loop, periodically checking stock prices. [cite: 1, 35]

## Extra Features

  * [cite\_start]**Confirmation Email on Setup**: An email is sent upon application startup to confirm that the email sending functionality is correctly configured. [cite: 74]
  * **Formatted Email Content**: Email notifications are HTML-formatted for better readability and include relevant stock information and an icon (if available).
  * **Input Validation**: Comprehensive validation of command-line arguments to ensure correct usage and prevent errors.

## Future Improvements

  * [cite\_start]**WebSockets for Real-time Data**: Ideally, integrate with an API that supports WebSockets for real-time stock data updates, as HTTP requests are less efficient for continuous monitoring. [cite: 67] [cite\_start](Note: Current API limitations necessitate HTTP polling [cite: 68]).
  * **Robust Error Logging**: Implement a more sophisticated logging mechanism (e.g., Serilog, NLog) for better error tracking and debugging.
  * **User Authentication for Email**: Instead of directly using SMTP password, explore more secure authentication methods for email sending (e.g., OAuth2).
  * **Dynamic Configuration Reloading**: Allow `AppSettings.json` to be reloaded at runtime without restarting the application.
  * **Multiple Asset Monitoring**: Extend the application to monitor multiple assets simultaneously.
  * **Database Integration**: Store alert history and configuration in a database.
  * **User Interface**: Develop a simple GUI or web interface for easier configuration and monitoring.
  * **Improved Email Templating**: Externalize email templates for easier customization.

## License

This project is licensed under the MIT License - see the [LICENSE](https://www.google.com/search?q=LICENSE) file for details.

## Contact

Gabriel da Silva Lomenha - gabriellomenha@gmail.com

Project Link: [https://github.com/GbrLomenha/stock-quote](https://github.com/GbrLomenha/stock-quote)