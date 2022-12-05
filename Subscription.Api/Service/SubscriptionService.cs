using Subscription.Api.Model;
using Subscription.DataAccess;
using System.Data;
using System.Data.SqlClient;

namespace Subscription.Api.Service
{
    public class SubscriptionService:ISubscription
    {
        private ISqlHelper _objSqlHelper = null;
        public SubscriptionService(ISqlHelper objSqlHelper)
        {
            _objSqlHelper = objSqlHelper;
        }
        public DataTable GetSubscriptions()
        {

            DataSet ds = _objSqlHelper.ExecuteDataSet("GETSUBSCRIPTIONS");
            return ds.Tables[0];
        }

        public bool CreateSubscription(SubscriptionDTO subscriptionDTO)
        {
            SqlParameter[] sqlParameter =
            {
                new SqlParameter("@BookId",subscriptionDTO.BookId),
                new SqlParameter("@SubscriberName",subscriptionDTO.SubscriberName),
                new SqlParameter("@DateSubscribed",subscriptionDTO.DateSubscribed),
            };
            bool ret = _objSqlHelper.ExecuteNonQuery("SaveSubscription", sqlParameter);
            return ret;
        }
    }
}
