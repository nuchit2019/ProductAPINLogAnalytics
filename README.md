# ProductAPINLogAnalytics

## Overview

This project integrates Azure Log Analytics with a .NET application to log various events and data. It uses the `LogAnalytics.Client` library to send logs to Azure Log Analytics.

![image](https://github.com/user-attachments/assets/482ed22d-1b76-495e-9761-5123e1673510)

![image](https://github.com/user-attachments/assets/f228c2e0-5d93-4688-bfc0-72868825b86e)

## Installation

1. **Clone the repository:**
    ```sh
    git clone https://github.com/nuchit2019/ProductAPINLogAnalytics.git
    ```

2. **Navigate to the project directory:**
    ```sh
    cd ProductAPINLogAnalytics
    ```

3. **Install dependencies:**
    ```sh
    dotnet restore
    ```

4. **Install `LogAnalytics.Client` and `NLog` packages:**
    ```sh
    dotnet add package LogAnalytics.Client
    dotnet add package NLog.Web.AspNetCore
    ```

## Configuration

1. **Update the `appsettings.json` file with your Azure Log Analytics workspace details:**
    ```json
    {
        "LogAnalytics": {
            "WorkspaceId": "your-workspace-id",
            "SharedKey": "your-shared-key",
            "LogType": "your-log-type"
        }
    }
    ```

2. **Add the NLog configuration to your project:**

    Create an `NLog.config` file with the following content:

    ```xml
    <?xml version="1.0" encoding="utf-8" ?>
    <nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
          xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

        <targets>
            <!-- File Target: Write logs to a file -->
            <target xsi:type="File"
                    name="logfile"
                    fileName="${basedir}/Logs/tni_nlog_app_${shortdate}.log"
                    layout="[${longdate}] [${uppercase:${level}}] ${message} ${exception:format=tostring}"
                    archiveAboveSize="5242880"
                    archiveNumbering="Sequence"
                    archiveFileName="${basedir}/Logs/tni_nlog_app_${shortdate}.{#}.log"
                    maxArchiveFiles="5" />

            <!-- Console Target: Write logs to the console -->
            <target xsi:type="Console" 
                    name="console"
                    layout="${longdate} [${level:uppercase=true}] [${callsite}] ${message} - Product: ${event-properties:item=productName}" />

           
           </targets>

        <rules>
            <!-- Log to Console and File -->
            <logger name="*" minlevel="Debug" writeTo="logfile" />
            <logger name="*" minlevel="Info" writeTo="console,logfile" />

            <!-- Log to Azure Log Analytics -->
            <logger name="*" minlevel="Info" writeTo="loganalytics" />
        </rules>

    </nlog>
    ```

## Usage

### Logging a Single Object

To log a single object, use the `LogObjectAsync` method:

```csharp
var logEntry = new { Message = "This is a log message", Level = "Info" };
await logAnalyticsService.LogObjectAsync(logEntry);
```

### Logging a List of Objects

To log a list of objects, use the `LogListObjectAsync` method:

```csharp
var logEntries = new List<object>
{
    new { Message = "First log message", Level = "Info" },
    new { Message = "Second log message", Level = "Error" }
};
await logAnalyticsService.LogListObjectAsync(logEntries);
```

### Extension Methods

Use the extension methods to create log entries with additional details:

```csharp
var logInfo = logAnalyticsService.LogInfo("Info message", new { Detail = "Some details" });
var logError = logAnalyticsService.LogError("Error message", new Exception("An error occurred"), new { Detail = "Error details" });
```

### Formatting Messages

Format log messages with contextual information:

```csharp
var formattedMessage = logAnalyticsService.FormatMessage("This is a log message");
Console.WriteLine(formattedMessage);
```

### Example: Logging in CreateProduct Method



The following example demonstrates the logging steps in the `CreateProduct` method:

### Code Flow

1. **Start Process Logging:**

```csharp
_logger.LogInformation(_logService.FormatMessage($"START Process CreateProduct: {product.Name}"));
var objLog = _logService.LogInfo(_logService.FormatMessage("START Process CreateProduct"), product);
lslogEntry.Add(objLog);

```

2. **Business Logic Execution:**
```csharp
    /* START Business logic Code...**/

    //Business logic placeholder

     /*END Business logic Code... */
```

3. **End Process Logging:**
    ```csharp
    _logger.LogInformation(_logService.FormatMessage("Product created successfully: {ProductName}", product.Name));
    objLog = _logService.LogInfo(_logService.FormatMessage("Product created successfully"), createdProduct);
    lslogEntry.Add(objLog);
    ```

4. **Error Handling:**
    ```csharp
    string errorMessage = $"Error Process CreateProduct(...): {product.Name}";
    var createdProductErr = new
    {
        Trace = ex.StackTrace,
        ExceptionMessage = ex.Message,
        ErrorMessage = errorMessage,
        Product = createdProduct
    };

    _logger.LogError(_logService.FormatMessage($"{errorMessage}, Exception: {ex.Message}, StackTrace: {ex.StackTrace}"), ex);
    var objLog = _logService.LogError(_logService.FormatMessage($"{errorMessage}, Exception: {ex.Message}, StackTrace: {ex.StackTrace}"), ex, createdProductErr);
    lslogEntry.Add(objLog);
    ```

5. **Send Logs to Azure Log Analytics:** Sending logs in batch
    ```csharp
    if (lslogEntry.Count > 0)
    {
        await _logService.LogListObjectAsync(lslogEntry);
    }
    ```

## API Documentation

This document provides details about the `CreateProduct` API, including its functionality, code structure, and logging strategy.

### Overview

The `CreateProduct` API endpoint is used to create a new product. It logs the process at various stages and handles errors gracefully.

### Endpoint

`POST /api/products`

### Request Body

The request body should be a JSON object representing the product to be created.

```json
{
  "name": "string",
  "price": "decimal"
}
```

### Response

- **201 Created**: Returns the created product.
- **500 Internal Server Error**: Returns an error message if the product creation fails.

### Logging Strategy

The method logs the following stages:

1. **START Process CreateProduct**:
    - Logs the start of the product creation process.
2. **Business logic**:
    - Placeholder for business logic.
3. **Product created successfully**:
    - Logs the successful creation of the product.
4. **Error Process CreateProduct**:
    - Logs error details if an exception occurs, including stack trace and exception message.
5. **END Process CreateProduct**:
    - Logs the end of the product creation process.
    - Sends all log entries to the analytics service.

This structured code ensures that each step of the process (start, success, error, and end) is logged appropriately for better traceability and debugging.

### Code Flow

#### Initialization
- Prepares a `List<object>` for collecting log entries.
- Simulates the creation of a `Product` object.

#### Try Block
- Executes the main business logic for product creation.
- Logs success messages if the process completes without errors.

#### Catch Block
- Captures and logs exceptions, along with detailed error information.
- Returns a `500 Internal Server Error` response.

#### Finally Block
- Logs the end of the process.
- Sends all collected log entries to the analytics service.

### Example

#### Request

```http
POST /api/products
Content-Type: application/json

{
  "name": "Sample Product",
  "price": 19.99
}
```

#### Response

```http
HTTP/1.1 201 Created
Content-Type: application/json

{
  "id": 1,
  "name": "Sample Product",
  "price": 19.99
}
```

### Error Handling

If an error occurs during the product creation process, the method logs the error and returns a 500 status code with an error message.

#### Example Error Response

```http
HTTP/1.1 500 Internal Server Error
Content-Type: application/json

{
  "errorMessage": "Error Process CreateProduct: Sample Product",
  "exceptionMessage": "Test exception for logging.",
  "trace": "Stack trace details..."
}
```

### Logging to Log Analytics

The method sends logs to Log Analytics in batches. If there are any log entries, they are sent asynchronously at the end of the process.

### Dependencies

- `_logger`: Used for logging to the console.
- `_logService`: Used for formatting messages
