using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace V.Server.Services
{
    public class SMSHttpClient : IDisposable
    {
        private const string _TOKEN = "8ad0ec02284b7152509bf6a1f168a26735770a8b";
        private HttpClient _client;

        public SMSHttpClient()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://http-api.smsmanager.cz/");
        }

        public void Dispose()
        {
            if (_client != null)
                _client.Dispose();
        }

        public async Task SendSMS(string sms, Action<string> onException)
        {
            var request = $"Send?apikey={_TOKEN}&number=420605203192&message={sms}";
            var response = await _client.GetAsync(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var errMessage = $"Send SMS Failed: {response.StatusCode}; {content}";
                onException.Invoke(errMessage);
            }
        }
    }
}
