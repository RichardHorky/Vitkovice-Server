using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace V.Server
{
    public class AppMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Data.ChangeNotifier _changeNotifier;
        private readonly ILogger<AppMiddleware> _logger;

        public AppMiddleware(RequestDelegate next, Data.ChangeNotifier changeNotifier, ILogger<AppMiddleware> logger)
        {
            _next = next;
            _changeNotifier = changeNotifier;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            System.Diagnostics.Debug.WriteLine($"Request {httpContext.Request.Path}");

            if (httpContext.Request.Path.ToString().ToUpper().Contains("/API/"))
            {
                var list = new List<string>();
                list.Add($"Path: {httpContext.Request.Path}");
                list.Add($"QueryString: {httpContext.Request.QueryString.Value}");
                list.Add($"Protocol: {httpContext.Request.Protocol}");
                list.Add($"Method: {httpContext.Request.Method}");
                list.Add($"IsHttps: {httpContext.Request.IsHttps}");
                foreach (var item in httpContext.Request.Headers)
                    list.Add($"{item.Key} = {item.Value}");
                _logger.LogDebug(string.Join('\n', list));
                _changeNotifier.OnNotify(list);
            }

            await _next(httpContext);
        }
    }
}
