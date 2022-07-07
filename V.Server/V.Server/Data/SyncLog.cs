using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V.Server.Data
{
    public class SyncLog : List<LogItem>
    {
        private const int _LOG_LENGTH = 100;
        private readonly ChangeNotifier _changeNotifier;

        public SyncLog(ChangeNotifier changeNotifier)
        {
            _changeNotifier = changeNotifier;
        }

        public void Log(TransferData.PanelItems<TransferData.InputStatusEnum> inputs, TransferData.PanelItems<TransferData.OutputStatusEnum> outputs)
        {
            if (TestIsChanged(inputs) || TestIsChanged(outputs))
            {
                var logItem = new LogItem()
                {
                    Date = DateTime.Now,
                    Inputs = inputs,
                    Outputs = outputs
                };
                this.Add(logItem);
                _changeNotifier.OnNotify(logItem);
            }

            if (this.Count > _LOG_LENGTH)
                this.RemoveAt(0);
        }

        private bool TestIsChanged<T>(TransferData.PanelItems<T> data) where T: Enum
        {
            var last = this.LastOrDefault();
            if (last == null)
                return true;

            TransferData.PanelItems<T> logItems = null;
            if (typeof(T) == typeof(TransferData.InputStatusEnum))
                logItems = last.Inputs as TransferData.PanelItems<T>;
            else if (typeof(T) == typeof(TransferData.OutputStatusEnum))
                logItems = last.Outputs as TransferData.PanelItems<T>;

            if (logItems == null)
                return false;

            var enumItems = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
            for (int i = 0; i < enumItems.Length; i++)
            {
                if (logItems.GetState(enumItems[i]) != data.GetState(enumItems[i]))
                    return true;
            }

            return false;
        }
    }
}
