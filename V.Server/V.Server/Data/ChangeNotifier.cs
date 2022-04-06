using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V.Server.Data
{
    public class ChangeNotifier
    {
        public event EventHandler<object> Notify;
        public void OnNotify(object item)
        {
            Notify?.Invoke(this, item);
        }

        public bool WatchDogEnabled { get; set; } = true;
    }
}
