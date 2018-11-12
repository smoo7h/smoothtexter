using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SmoothTxtr
{
    public static class Function1
    {
        [FunctionName("sendmessage")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("processing txt message");

            // parse query parameter
            string tonum = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "tonum", true) == 0)
                .Value;

            if (tonum == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                tonum = data?.name;
            }

            // parse query parameter
            string fromnum = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "fromnum", true) == 0)
                .Value;

            if (fromnum == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                tonum = data?.fromnum;
            }

            // parse query parameter
            string message = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "message", true) == 0)
                .Value;

            if (message == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                tonum = data?.message;
            }



            //set up twillo
            // Find your Account Sid and Token at twilio.com/console
            const string accountSid = "";
            const string authToken = "";

            TwilioClient.Init(accountSid, authToken);


            var txtmessage = MessageResource.Create(
                body: message,
                from: new Twilio.Types.PhoneNumber(fromnum),
                to: new Twilio.Types.PhoneNumber(tonum)
            );

            log.Info(txtmessage.Sid);
           


            return tonum == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "")
                : req.CreateResponse(HttpStatusCode.OK, "sent message to " + tonum);
        }
    }
}
