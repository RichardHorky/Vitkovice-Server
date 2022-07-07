using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V.Server.Data
{
    public class LogItem
    {
        public DateTime Date { get; set; }
        public TransferData.PanelItems<TransferData.InputStatusEnum> Inputs { get; set; } = new TransferData.PanelItems<TransferData.InputStatusEnum>();
        public TransferData.PanelItems<TransferData.OutputStatusEnum> Outputs { get; set; } = new TransferData.PanelItems<TransferData.OutputStatusEnum>();
    }
}
