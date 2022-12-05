using System.ComponentModel.DataAnnotations;

namespace Subscription.Api.Model
{
    public class SubscriptionDTO
    {
        [Required]
        public string SubscriberName { get; set; }

        [Required]
        public string BookId { get; set; }

        [Required]
        public DateTime DateSubscribed { get; set; }

    }
}
