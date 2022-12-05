using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Polly;
using Subscription.Api.Controllers;
using Subscription.Api.Model;
using Subscription.Api.Service;

namespace Subscription.Test
{
    [TestClass]
    public class SubscriptionTest
    {
        private Mock<ISubscription>? _subscription;
        private Mock<IHttpClientFactory>? _httpClientFactory;
        private Mock<IAsyncPolicy<HttpResponseMessage>>? _asyncPolicy;

        private SubscriptionController? _subscriptionController;

        [TestInitialize]
        public void GetTestInitialize()
        {
            _subscription = new Mock<ISubscription>();
            var mock = new Mock<ILogger<SubscriptionController>>();
            ILogger<SubscriptionController> logger = mock.Object;
            logger = Mock.Of<ILogger<SubscriptionController>>();
            _httpClientFactory = new Mock<IHttpClientFactory>();
            _asyncPolicy = new Mock<IAsyncPolicy<HttpResponseMessage>>();
            _subscriptionController = new SubscriptionController(_subscription.Object, logger, _httpClientFactory.Object, _asyncPolicy.Object);
        }

        [TestMethod]
        public void GetSubscriptions()
        {            
            // Act on Test  
            var response = _subscriptionController.GetSubscriptions().GetAwaiter().GetResult() as ObjectResult; ;
          
            // Assert the result  
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.StatusCode);
            Assert.AreEqual(200, (int)response.StatusCode);
        }

        [TestMethod]
        public void CreateSubscription()
        {
            // Act on Test  
            var response = _subscriptionController.CreateSubscription(new SubscriptionDTO() { BookId = "test", DateSubscribed= Convert.ToDateTime("12/12/2022"),SubscriberName="test"}).GetAwaiter().GetResult() as ObjectResult; ;

            // Assert the result  
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.StatusCode);
            Assert.AreEqual(422, (int)response.StatusCode);
        }
    }
}