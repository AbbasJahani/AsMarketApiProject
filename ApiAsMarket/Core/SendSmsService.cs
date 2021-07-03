using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ApiAsMarket.Core
{
    public class Values
    {
        public string verificationcode { get; set; }
        public string password { get; set; }
        public string username { get; set; }
    }

    public class SmsBody
    {
        public string pattern_code { get; set; }
        public string originator { get; set; }
        public string recipient { get; set; }
        public Values values { get; set; }
    }



   
    public class SendSmsService
    {
        private static readonly HttpClient client = new HttpClient();


        public async void SendForgotPassWordSms(string recipient, string password, string username, string verificationcode)
        {
            var smsBody = new SmsBody();
            var values = new Values();
            smsBody.pattern_code = "t2cfmnyo0c";
            smsBody.originator = "+9850002";
            smsBody.recipient = recipient;


            values.password = password;
            values.username = username;
            values.verificationcode = verificationcode;


            smsBody.values = new Values();
            smsBody.values = values;


            var json = JsonConvert.SerializeObject(smsBody, Formatting.Indented);

            var stringContent = new StringContent(json);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string apiKey = "1sZ53twJL2gAwjsyMtZS5Ink18MCcF24653WYy7OOJ4=";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("apikey", "=" + apiKey);
            var response = await client.PostAsync("http://rest.ippanel.com/v1/messages/patterns/send", stringContent);

            var responseString = await response.Content.ReadAsStringAsync();
        
        }

    }
}
