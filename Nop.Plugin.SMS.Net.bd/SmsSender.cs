using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nop.Plugin.SMS.Alpha
{
    public enum SmsType : int
    {
        //Ok
        Draft = 69,
        Default = 70,
        Sending = 71,
        Active = 72,
        Failed = 73,
        SystemError = 74,

    }
    public class SmsSender : ISmsSender
    {
        private readonly IConfiguration _configuration;
        public SmsSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public SmsType FindById(long ids)
        {
            using (var client = new HttpClient())
            {
                int TimeWait = Convert.ToInt32(_configuration["SMS:Time"]); //set deafult value
                Thread.Sleep(TimeWait);
                client.BaseAddress = new Uri(_configuration["SMS:Url"]);//set deafult value
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync("index.php?app=ws&u=" + _configuration["SMS:User"] + "&h=" + _configuration["SMS:Password"] + "&op=ds&smslog_id=" + ids + "&format=xml").Result;// set deafult value
                using (HttpContent content = response.Content)
                {
                    Task<string> value = content.ReadAsStringAsync();
                    XDocument doc = XDocument.Parse(value.Result);
                    string sysError = doc.Descendants("response").Descendants("status").FirstOrDefault().Value;
                    if (sysError == "ERR")
                        return SmsType.SystemError;

                    string elementVal = doc.Descendants("response").Descendants("data").Descendants("item").Descendants("status").FirstOrDefault().Value;
                    switch (elementVal)
                    {
                        case "0":
                            return SmsType.Sending;
                        case "1":
                            return SmsType.Active;
                        case "2":
                            return SmsType.Failed;
                        default:
                            return SmsType.SystemError;

                    }

                }
            }
        }
        public SmsType SendSmsAsync(string num, string meg, string baseUrl, string api_key, string sender_id = null)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync("?api_key=" + api_key + "&msg=" + meg + "&to=" + num + "&sender_id=" + sender_id).Result;
                using (HttpContent content = response.Content)
                {
                    var bkresult = content.ReadAsStringAsync().Result;
                    dynamic stuff = JsonConvert.DeserializeObject(bkresult);
                    if (stuff.error == "0")
                    {
                        return SmsType.Sending;
                    }
                    else
                    {
                        return SmsType.SystemError;
                    }

                }
            }
        }
        ////Old SMS API
        //public SmsType SendSmsAsync(string num, string meg, SmsType ProcessType)
        //{
        //    using (var client = new HttpClient())
        //    {

        //        client.BaseAddress = new Uri(_configuration["SMS:Url"]);// // 
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        var response = client.GetAsync("index.php?app=ws&u=" + _configuration["SMS:User"] + "&h=" + SecurityMethod.DecryptString(_configuration["SMS:Password"], Key.secretKeyAPP) + "&format=xml&op=pv&to=" + num + "&msg=" + meg).Result;

        //        using (HttpContent content = response.Content)
        //        {

        //            Task<string> result = content.ReadAsStringAsync();
        //            XDocument doc = XDocument.Parse(result.Result);
        //            //System Valid Data
        //            string sysError = doc.Descendants("response").Descendants("status").FirstOrDefault().Value;
        //            if (sysError == "ERR")
        //                return SmsType.SystemError;
        //            else
        //            {

        //                if (ProcessType == SmsType.Sending)
        //                {
        //                    return SmsType.Sending;
        //                    //smslog_id return database
        //                }
        //                else
        //                {
        //                    string sms_id = doc.Descendants("response").Descendants("data").Descendants("item").Descendants("smslog_id").FirstOrDefault().Value;
        //                    SmsType smsStatus = FindById(Convert.ToInt64(sms_id));
        //                    switch (smsStatus.ToString())
        //                    {
        //                        case "0":
        //                            return SmsType.Sending;
        //                        case "1":
        //                            return SmsType.Active;
        //                        //smslog_id return database
        //                        case "2":
        //                            return SmsType.Failed;
        //                        default:
        //                            return SmsType.SystemError;

        //                    }
        //                }

        //            }


        //        }
        //    }
        //}
    }

}