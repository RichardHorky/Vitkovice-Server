using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V.Server.Data
{
    public class ErrorModel
    {
        public ErrorModel(string message)
        {
            Date = DateTime.UtcNow;
            Text = message;
        }
        public DateTime Date { get; private set; }
        public string Text { get; set; }
    }
}
