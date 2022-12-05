using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polly;
using Subscription.Api.Model;
using Subscription.Api.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Subscription.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionController : Controller
    {
        private ISubscription _objSubscription;
        private readonly ILogger<SubscriptionController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAsyncPolicy<HttpResponseMessage> _policy;

        public SubscriptionController(ISubscription objSubscription, ILogger<SubscriptionController> logger, IHttpClientFactory httpClientFactory, IAsyncPolicy<HttpResponseMessage> policy)
        {
            _objSubscription = objSubscription;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _policy = policy;
        }
        /// <summary>
        /// Get All Subscription
        /// </summary>
        /// <returns>Returns list of subscription</returns>
        [HttpGet]
        [Route("GetSubscriptions")]
        public async Task<IActionResult> GetSubscriptions()
        {
            DataTable dt;
            try
            {
                dt = _objSubscription.GetSubscriptions();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                throw;
            }

            return Ok(JsonConvert.SerializeObject(dt));
        }
        /// <summary>
        /// Create a new subscription
        /// </summary>
        /// <param name="subscriptionDTO">Input Data Model</param>
        /// <returns>retuns 200 while subscription created successfully</returns>
        [HttpPost]
        [Route("CreateSubscription")]
        public async Task<IActionResult> CreateSubscription([FromBody] SubscriptionDTO subscriptionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool bookAvailable = false;
            bool returnValue = false;
            try
            {
                
                var client = _httpClientFactory.CreateClient("Book.Api");
                var response = await _policy.ExecuteAsync(() => client.GetAsync("Book/BookAvailable/" + subscriptionDTO.BookId));
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    bookAvailable = Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
                }
                if (bookAvailable)
                {
                    returnValue = _objSubscription.CreateSubscription(subscriptionDTO);
                }
                else
                {
                    return UnprocessableEntity("Book Does Not Exist In The Library");
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                throw;
            }

            return Ok(returnValue);
        }
    }
}
