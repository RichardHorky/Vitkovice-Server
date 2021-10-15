using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using V.Server.Data;

namespace V.Server.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly Helpers.DateHelper _dateHelper;
        private readonly DataStorage _dataStorage;
        private readonly ChangeNotifier _changeNotifier;

        public SyncController(Helpers.DateHelper dateHelper, DataStorage dataStorage, ChangeNotifier changeNotifier)
        {
            _dateHelper = dateHelper;
            _dataStorage = dataStorage;
            _changeNotifier = changeNotifier;
        }

        public ActionResult<string> Get()
        {
            var seconds = _dateHelper.GetSeconds();
            var fnItems = _dataStorage.GetData<Data.TransferData.FnItems>();
            var fnItemsList = new List<string>()
            {
                seconds.ToString(),
                ((fnItems?.Source ?? TransferData.SourceEnum.Arduino) == TransferData.SourceEnum.Server && (fnItems?.Valid ?? false) ? fnItems.ID : string.Empty)
            };
            AddItemToList(fnItemsList, Data.TransferData.ButtonPressEnum.Termostat1, fnItems);
            AddItemToList(fnItemsList, Data.TransferData.ButtonPressEnum.Termostat2, fnItems);
            AddItemToList(fnItemsList, Data.TransferData.ButtonPressEnum.ElHeating, fnItems);
            AddItemToList(fnItemsList, Data.TransferData.ButtonPressEnum.Water, fnItems);
            AddItemToList(fnItemsList, Data.TransferData.ButtonPressEnum.Cams, fnItems);
            AddItemToList(fnItemsList, Data.TransferData.ButtonPressEnum.Alarm, fnItems);

            var resStr = string.Join('|', fnItemsList);

            return Ok($"{{{resStr}}}");
        }

        [HttpGet]
        [Route("Post/{data}")]
        public void Post(string data)
        {
            var fnItems = _dataStorage.GetData<TransferData.FnItems>();
            var list = data.Split('|');
            if (fnItems.Source == TransferData.SourceEnum.Server && fnItems.Valid && list[1] != fnItems.ID)
            {
                //is valid waiting - skip it
                return;
            }
            fnItems = new TransferData.FnItems();
            ProcessStates(list, fnItems);
            fnItems.Source = TransferData.SourceEnum.Arduino;
            _dataStorage.SaveData(fnItems);
            _changeNotifier.OnNotify(TransferData.SourceEnum.Arduino);
        }

        private void AddItemToList(List<string> list, Data.TransferData.ButtonPressEnum buttonPress, Data.TransferData.FnItems fnItems)
        {
            list.Add((fnItems?.Valid ?? false) ? ((byte)fnItems.GetState(buttonPress)).ToString() : string.Empty);
        }

        private void ProcessStates(string[] items, TransferData.FnItems fnItems)
        {
            ProcessState(items[2], TransferData.ButtonPressEnum.Termostat1, fnItems);
            ProcessState(items[3], TransferData.ButtonPressEnum.Termostat2, fnItems);
            ProcessState(items[4], TransferData.ButtonPressEnum.ElHeating, fnItems);
            ProcessState(items[5], TransferData.ButtonPressEnum.Water, fnItems);
            ProcessState(items[6], TransferData.ButtonPressEnum.Cams, fnItems);
            ProcessState(items[7], TransferData.ButtonPressEnum.Alarm, fnItems);
        }

        private void ProcessState(string value, TransferData.ButtonPressEnum buttonPress, TransferData.FnItems fnItems)
        {
            if (byte.TryParse(value, out byte iState))
            {
                fnItems.SetState(buttonPress, (TransferData.FnStateEnum)iState);
            }
        }
    }
}
