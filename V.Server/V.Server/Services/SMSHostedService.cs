using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using V.Server.Data;

namespace V.Server.Services
{
    public class SMSHostedService : HostedService
    {
        private readonly SMSHttpClient _smsHttpClient;
        private readonly Errors _errors;
        private readonly ChangeNotifier _changeNotifier;
        private readonly ILogger<SMSHostedService> _logger;
        private readonly GlobalData _globalData;
        private const int _loopMinutes = 5;

        public SMSHostedService(
            SMSHttpClient smsHttpClient,
            Errors errors,
            ChangeNotifier changeNotifier,
            ILogger<SMSHostedService> logger,
            GlobalData globalData)
        {
            _smsHttpClient = smsHttpClient;
            _errors = errors;
            _changeNotifier = changeNotifier;
            _logger = logger;
            _globalData = globalData;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(_loopMinutes * 60 * 1000);
                if (_changeNotifier.WatchDogEnabled)
                {
                    if (!cancellationToken.IsCancellationRequested)
                        await TestStatus();
                }
            }
        }

        private async Task TestStatus()
        {
            try
            {
                if ((DateTime.Now - _globalData.LastArduinoRequest).TotalMinutes > _loopMinutes)
                    await _smsHttpClient.SendSMS("Arduino zdechlo!", OnException);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _errors.ErrorList.Add(new Data.ErrorModel(ex.ToString()));
            }
        }

        private void OnException(string msg)
        {
            _logger.LogError(msg);
            _errors.ErrorList.Add(new Data.ErrorModel(msg));
        }
    }
}
