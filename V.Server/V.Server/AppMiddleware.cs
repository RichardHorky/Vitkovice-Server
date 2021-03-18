using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace V.Server
{
    public class AppMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Data.ChangeNotifier _changeNotifier;

        public AppMiddleware(RequestDelegate next, Data.ChangeNotifier changeNotifier)
        {
            _next = next;
            _changeNotifier = changeNotifier;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            System.Diagnostics.Debug.WriteLine($"Request {httpContext.Request.Path}");

            var list = new List<string>();
            list.Add($"Path: {httpContext.Request.Path}");
            list.Add($"QueryString: {httpContext.Request.QueryString.Value}");
            list.Add($"Protocol: {httpContext.Request.Protocol}");
            list.Add($"Method: {httpContext.Request.Method}");
            list.Add($"IsHttps: {httpContext.Request.IsHttps}");
            foreach (var item in httpContext.Request.Headers)
                list.Add($"{item.Key} = {item.Value}");
            _changeNotifier.OnNotify(list);

            await _next(httpContext);
        }
    }
}
