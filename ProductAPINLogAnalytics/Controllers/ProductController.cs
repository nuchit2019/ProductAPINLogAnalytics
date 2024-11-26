using Microsoft.AspNetCore.Mvc;
using ProductAPINLogAnalytics.Models;
using ProductAPINLogAnalytics.Services;

namespace ProductAPINLogAnalytics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogAnalyticsService _logService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogAnalyticsService logAnalyticsService, ILogger<ProductController> logger)
        {
            _logService = logAnalyticsService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            List<object> lslogEntry = new List<object>();
            var createdProduct = new Product { Id = 1, Name = product.Name, Price = product.Price };

            try
            {
                //====================================//
                //1. START Process CreateProduct
                //====================================//
                _logger.LogInformation(_logService.FormatMessage($"START Process CreateProduct: {product.Name}"));//.......................................................... Log Console
                var objLog = _logService.LogInfo(_logService.FormatMessage($"START Process CreateProduct"), createdProduct);//................................................ Log Analytics
                lslogEntry.Add(objLog);

                //====================================//
                //2. Business logic.....
                //====================================//
                /*START Business logic Code...*/

                //...
                //Business logic placeholder
                //...

                /* END Business logic Code...  */


                //====================================//
                //3. Successfully Process CreateProduct
                //====================================//
                _logger.LogInformation(_logService.FormatMessage("Product created successfully: {ProductName}", product.Name));//.................................................. Log Console
                objLog = _logService.LogInfo(_logService.FormatMessage("Product created successfully"), createdProduct);//......................................................... Log Analytics
                lslogEntry.Add(objLog);


                //throw new Exception("Test exception for logging.");

                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
                //====================================//
                //4. Error Process CreateProduct
                //====================================//
                string errorMessage = $"Error Process CreateProduct: {product.Name}";
                var createdProductErr = new
                {
                    Trace = ex.StackTrace,
                    ExceptionMessage = ex.Message,
                    ErrorMessage = errorMessage,
                    Product = createdProduct
                };

                _logger.LogError(_logService.FormatMessage($"{errorMessage}, Exception: {ex.Message}, StackTrace: {ex.StackTrace}"), ex);//........................................ Log Console
                var objLog = _logService.LogError(_logService.FormatMessage($"{errorMessage}, Exception: {ex.Message}, StackTrace: {ex.StackTrace}"), ex, createdProductErr);//.... Log Analytics
                lslogEntry.Add(objLog);

                return StatusCode(500, $"Internal server error: {errorMessage}");
            }
            finally
            {
                //====================================//
                //5. END Process CreateProduct
                //====================================//
                _logger.LogInformation(_logService.FormatMessage($"END Process CreateProduct: {product.Name}"));//................................................................. Log Console
                var objLog = _logService.LogInfo(_logService.FormatMessage($"END Process CreateProduct:ProductName {product.Name}"), createdProduct);//............................ Log Analytics
                lslogEntry.Add(objLog);

                //====================================//
                //6. Send List log to Log Analytics .... Sending logs in batch
                //====================================//
                if (lslogEntry.Count > 0)
                {
                    await _logService.LogListObjectAsync(lslogEntry);//............................................................................................................ Send List log to Log Analytics
                }

            }

        }


        [HttpGet("{id}")]
        public async Task <IActionResult> GetProduct(int id)
        {
            List<object> lslogEntry = new List<object>();
            // Simulate fetching product logic 
            var product = new Product { Id = id, Name = "Product A", Price = 123 };
            try
            {
                //====================================//
                //1. START Process CreateProduct
                //====================================//
                _logger.LogInformation(_logService.FormatMessage($"START Process GetProduct Id: {product.Id}")); 
                var objLog = _logService.LogInfo(_logService.FormatMessage($"START Process GetProduct:"), product); 
                lslogEntry.Add(objLog);

                //====================================//
                //2. Business logic.....
                //====================================//


                _logger.LogInformation(_logService.FormatMessage($"GetProduct Id successfully: {product.Id}"));//.................................................. Log Console
                objLog = _logService.LogInfo(_logService.FormatMessage($"GetProduct Id successfully: {product.Id}"), product);//......................................................... Log Analytics
                lslogEntry.Add(objLog);


            }
            catch (Exception ex)
            {
                //====================================//
                //3. Error Process CreateProduct
                //====================================//
                string errorMessage = $"Error Process GetProduct Id: {product.Id}";
                var createdProductErr = new
                {
                    Trace = ex.StackTrace,
                    ExceptionMessage = ex.Message,
                    ErrorMessage = errorMessage,
                    Product = product
                };

                _logger.LogError(_logService.FormatMessage($"{errorMessage}, Exception: {ex.Message}, StackTrace: {ex.StackTrace}"), ex);// Log Console
                var objLog = _logService.LogError(_logService.FormatMessage($"{errorMessage}, Exception: {ex.Message}, StackTrace: {ex.StackTrace}"), ex, product);// Log Analytics
                lslogEntry.Add(objLog);

                return StatusCode(500, $"Internal server error: {errorMessage}");


            }
            finally
            {

                //====================================//
                //4. END Process CreateProduct
                //====================================//
                _logger.LogInformation(_logService.FormatMessage($"END Process GetProduct: {product.Id}"));// Log Console
                var objLog = _logService.LogInfo(_logService.FormatMessage($"END Process GetProduct:ProductId {product.Id}"), product);// Log Analytics
                lslogEntry.Add(objLog);

                //====================================//
                //5. Send List log to Log Analytics
                //====================================//
                if (lslogEntry.Count > 0)
                {
                    await _logService.LogListObjectAsync(lslogEntry);// Send List log to Log Analytics
                }

            }
            _logger.LogInformation("Fetching product with ID: {ProductId}", id);

          
            return Ok(product);
        }


        [HttpGet()]
        public async Task<IActionResult> GetProducts()
        {
            List<object> lslogEntry = new List<object>();
            // Simulate fetching product logic 
            var product = new Product { Id = 55, Name = "Product A", Price = 123 };
            try
            {
                //====================================//
                //1. START Process CreateProduct
                //====================================//
                _logger.LogInformation(_logService.FormatMessage($"START Process GetProducts"));
                var objLog = _logService.LogInfo(_logService.FormatMessage($"START Process GetProducts:"), product);
                lslogEntry.Add(objLog);

                //====================================//
                //2. Business logic.....
                //====================================//
                throw new Exception("GetProducts Test exception for logging.");

                _logger.LogInformation(_logService.FormatMessage($"GetProducts Id successfully"));//.................................................. Log Console
                objLog = _logService.LogInfo(_logService.FormatMessage($"GetProducts Id successfully"), product);//......................................................... Log Analytics
                lslogEntry.Add(objLog);


            }
            catch (Exception ex)
            {
                //====================================//
                //3. Error Process CreateProduct
                //====================================//
                string errorMessage = $"Error Process GetProducts ";
                var createdProductErr = new
                {
                    Trace = ex.StackTrace,
                    ExceptionMessage = ex.Message,
                    ErrorMessage = errorMessage,
                    Product = product
                };

                _logger.LogError(_logService.FormatMessage($"{errorMessage}, Exception: {ex.Message}, StackTrace: {ex.StackTrace}"), ex);// Log Console
                var objLog = _logService.LogError(_logService.FormatMessage($"{errorMessage}, Exception: {ex.Message}, StackTrace: {ex.StackTrace}"), ex, product);// Log Analytics
                lslogEntry.Add(objLog);

                return StatusCode(500, $"Internal server error: {errorMessage}");


            }
            finally
            {

                //====================================//
                //4. END Process CreateProduct
                //====================================//
                _logger.LogInformation(_logService.FormatMessage($"END Process GetProducts "));// Log Console
                var objLog = _logService.LogInfo(_logService.FormatMessage($"END Process GetProducts"), product);// Log Analytics
                lslogEntry.Add(objLog);

                //====================================//
                //5. Send List log to Log Analytics
                //====================================//
                if (lslogEntry.Count > 0)
                {
                    await _logService.LogListObjectAsync(lslogEntry);// Send List log to Log Analytics
                }

            }          


            return Ok(product);
        }


    }
}
