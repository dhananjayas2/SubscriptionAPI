using Subscription.Api.Model;
using System.Data;

namespace Subscription.Api.Service
{
    public interface ISubscription
    {
        DataTable GetSubscriptions();
        bool CreateSubscription(SubscriptionDTO subscriptionDTO);
    }
}
