using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V.Server.Data
{
    public class DataChangedArgs
    {
        public TransferData.FnItems FnItems { get; set; }
        public TransferData.CmdItems CmdItems { get; set; }
    }
}
