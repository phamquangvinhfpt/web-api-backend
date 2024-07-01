
using System.Net;
using Microsoft.Extensions.Options;
using static Core.Infrastructure.SpeedSMS.SpeedSMSType;
namespace Core.Infrastructure.SpeedSMS
{
    public class SpeedSMSService : ISpeedSMSService
    {
        private String rootURL;
        private String accessToken;
        private String sender;

        public SpeedSMSService(IOptions<SpeedSMSSettings> settings)
        {
            this.rootURL = settings.Value.RootUrl!;
            this.accessToken = settings.Value.AccessToken!;
            this.sender = settings.Value.Sender!;
        }

        //hàm sendSMS sẽ trả về một json string như sau:*/
        //{
        //   "status": "success", "code": "00", 
        //   "data": {
        //    "tranId": 123456, "totalSMS": 1,     
        //     "totalPrice": 250, "invalidPhone": []
        //}

        public String sendSMS(String[] phones, String content, int type)
        {
            String url = rootURL + "/sms/send";
            if (phones.Length <= 0)
                return "";
            if (content.Equals(""))
                return "";

            if (type == TYPE_BRANDNAME && sender.Equals(""))
                return "";

            NetworkCredential myCreds = new NetworkCredential(accessToken, ":x");
            WebClient client = new WebClient();
            client.Credentials = myCreds;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            string builder = "{\"to\":[";

            for (int i = 0; i < phones.Length; i++)
            {
                builder += "\"" + phones[i] + "\"";
                if (i < phones.Length - 1)
                {
                    builder += ",";
                }
            }
            builder += "], \"content\": \"" + Uri.EscapeDataString(content) + "\", \"type\":" + type + ", \"sender\": \"" + sender + "\"}";

            String json = builder.ToString();
            return client.UploadString(url, json);
        }
    }
}