using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace V.Server.Services
{
    public class SMSHostedService : HostedService
    {
        private readonly SMSHttpClient _smsHttpClient;
        private readonly Data.Errors _errors;
        private readonly IServiceProvider _serviceProvider;
        private const int _loopMinutes = 5;

        public SMSHostedService(
            SMSHttpClient smsHttpClient,
            Data.Errors errors,
            IServiceProvider serviceProvider)
        {
            _smsHttpClient = smsHttpClient;
            _errors = errors;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(_loopMinutes * 60 * 1000);
                if (!cancellationToken.IsCancellationRequested)
                    await TestStatus();
            }
        }

        private async Task TestStatus()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dataStorage = scope.ServiceProvider.GetRequiredService<Data.DataStorage>();
                    var fnItems = dataStorage.GetData<Data.TransferData.FnItems>();
                    if ((DateTime.UtcNow - fnItems.Date).TotalMinutes > _loopMinutes)
                        await _smsHttpClient.SendSMS("Arduino zdechlo!", OnException);
                }
            }
            catch (Exception ex)
            {
                _errors.ErrorList.Add(new Data.ErrorModel(ex.ToString()));
            }
        }

        private void OnException(string msg)
        {
            _errors.ErrorList.Add(new Data.ErrorModel(msg));
        }
    }
}
