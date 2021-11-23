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
        private readonly Errors _errors;
        private const string _TOKEN_PASSWORD = "x4tr5Gj";
        private const string _INVALID_TOKEN = "invalid_token";

        public SyncController(Helpers.DateHelper dateHelper, DataStorage dataStorage, ChangeNotifier changeNotifier, Errors errors)
        {
            _dateHelper = dateHelper;
            _dataStorage = dataStorage;
            _changeNotifier = changeNotifier;
            _errors = errors;
        }

        public ActionResult<string> Get()
        {
            try
            {
                var seconds = _dateHelper.GetSeconds();
                var fnItems = _dataStorage.GetData<Data.TransferData.FnItems>();
                var itemsList = new List<string>()
            {
                GetToken(),
                seconds.ToString(),
                ((fnItems?.Source ?? TransferData.SourceEnum.None) == TransferData.SourceEnum.Server && (fnItems?.Valid ?? false) ? fnItems.ID : string.Empty)
            };
                AddItemToList(itemsList, Data.TransferData.ButtonPressEnum.Termostat1, fnItems);
                AddItemToList(itemsList, Data.TransferData.ButtonPressEnum.Termostat2, fnItems);
                AddItemToList(itemsList, Data.TransferData.ButtonPressEnum.ElHeating, fnItems);
                AddItemToList(itemsList, Data.TransferData.ButtonPressEnum.Water, fnItems);
                AddItemToList(itemsList, Data.TransferData.ButtonPressEnum.Cams, fnItems);
                AddItemToList(itemsList, Data.TransferData.ButtonPressEnum.Alarm, fnItems);

                var cmdItems = _dataStorage.GetData<TransferData.CmdItems>();
                itemsList.Add((cmdItems?.Source ?? TransferData.SourceEnum.None) == TransferData.SourceEnum.Server && (cmdItems?.Valid ?? false) ? cmdItems.ID : string.Empty);
                AddCmdItemToList(itemsList, TransferData.ButtonPressEnum.GSM, cmdItems);
                AddCmdItemToList(itemsList, TransferData.ButtonPressEnum.WIFI, cmdItems);
                AddCmdItemToList(itemsList, TransferData.ButtonPressEnum.AlarmOff, cmdItems);

                var resStr = string.Join('|', itemsList);

                return Ok($"{{{resStr}}}");
            }
            catch (Exception ex)
            {
                _errors.ErrorList.Add(new ErrorModel(ex.ToString()));
            }
            return Ok();
        }

        [HttpGet]
        [Route("Post/{data}")]
        public ActionResult Post(string data)
        {
            try
            {
                var list = data.Split('|');
                if (list.Length == 0 || !CheckToken(list[0]))
                    return Ok();

                var fnItems = _dataStorage.GetData<TransferData.FnItems>() ?? new TransferData.FnItems();
                var validWaiting = fnItems.Source == TransferData.SourceEnum.Server && fnItems.Valid && list[2] != fnItems.ID;
                //is valid waiting - skip it
                if (!validWaiting)
                {
                    fnItems = new TransferData.FnItems();
                    ProcessStates(list, fnItems);
                    fnItems.Source = TransferData.SourceEnum.Arduino;
                    fnItems.Date = DateTime.Now;
                    _dataStorage.SaveData(fnItems);
                    //save to recovery after terminal has lost power
                    _dataStorage.SaveData(fnItems, "FnRecovery");
                }

                var cmdItems = _dataStorage.GetData<TransferData.CmdItems>() ?? new TransferData.CmdItems();
                validWaiting = cmdItems.Source == TransferData.SourceEnum.Server && cmdItems.Valid && list[9] != cmdItems.ID;
                //is valid waiting - skip it
                if (!validWaiting)
                {
                    cmdItems = new TransferData.CmdItems();
                    ProcessStates(list, cmdItems);
                    cmdItems.Source = TransferData.SourceEnum.Arduino;
                    cmdItems.Date = DateTime.Now;
                    _dataStorage.SaveData(cmdItems);
                }

                var inputItems = new TransferData.PanelItems<TransferData.InputStatusEnum>();
                ProcessStates(13, list, inputItems);
                _dataStorage.SaveData(inputItems, "InputItems");

                var outputItems = new TransferData.PanelItems<TransferData.OutputStatusEnum>();
                ProcessStates(25, list, outputItems);
                _dataStorage.SaveData(outputItems, "OutputItems");

                var args = new DataChangedArgs() { FnItems = fnItems, CmdItems = cmdItems, InputItems = inputItems, OutputItems = outputItems };
                _changeNotifier.OnNotify(args);
            }
            catch (Exception ex)
            {
                _errors.ErrorList.Add(new ErrorModel(ex.ToString()));
            }

            return Ok();
        }

        [HttpGet]
        [Route("Recovery")]
        public ActionResult<string> Recovery()
        {
            try
            {
                var fnRecovery = _dataStorage.GetData<TransferData.FnItems>("FnRecovery");
                if (fnRecovery == null)
                    return Ok("{1|1|1|1|1|1}");

                var items = new List<string>();
                items.Add(((byte)fnRecovery.GetState(TransferData.ButtonPressEnum.Termostat1)).ToString());
                items.Add(((byte)fnRecovery.GetState(TransferData.ButtonPressEnum.Termostat2)).ToString());
                items.Add(((byte)fnRecovery.GetState(TransferData.ButtonPressEnum.ElHeating)).ToString());
                items.Add(((byte)fnRecovery.GetState(TransferData.ButtonPressEnum.Water)).ToString());
                items.Add(((byte)fnRecovery.GetState(TransferData.ButtonPressEnum.Cams)).ToString());
                items.Add(((byte)fnRecovery.GetState(TransferData.ButtonPressEnum.Alarm)).ToString());

                var resStr = string.Join('|', items);

                return Ok($"{{{resStr}}}");
            }
            catch (Exception ex)
            {
                _errors.ErrorList.Add(new ErrorModel(ex.ToString()));
            }
            return Ok("{}");
        }

        [HttpGet]
        [Route("Token")]
        public ActionResult<string> Token()
        {
            var token = GetToken();
            return Ok($"{{{token}}}");
        }

        private void AddItemToList(List<string> list, Data.TransferData.ButtonPressEnum buttonPress, Data.TransferData.FnItems fnItems)
        {
            list.Add((fnItems?.Source ?? TransferData.SourceEnum.Arduino) == TransferData.SourceEnum.Server && (fnItems?.Valid ?? false) ? ((byte)fnItems.GetState(buttonPress)).ToString() : string.Empty);
        }

        private void AddCmdItemToList(List<string> list, Data.TransferData.ButtonPressEnum buttonPress, Data.TransferData.CmdItems cmdItems)
        {
            list.Add((cmdItems?.Source ?? TransferData.SourceEnum.Arduino) == TransferData.SourceEnum.Server && (cmdItems?.Valid ?? false) ? (cmdItems.GetPressed(buttonPress) ? "1" : "0") : string.Empty);
        }

        private void ProcessStates(string[] items, TransferData.FnItems fnItems)
        {
            ProcessState(items[3], TransferData.ButtonPressEnum.Termostat1, fnItems);
            ProcessState(items[4], TransferData.ButtonPressEnum.Termostat2, fnItems);
            ProcessState(items[5], TransferData.ButtonPressEnum.ElHeating, fnItems);
            ProcessState(items[6], TransferData.ButtonPressEnum.Water, fnItems);
            ProcessState(items[7], TransferData.ButtonPressEnum.Cams, fnItems);
            ProcessState(items[8], TransferData.ButtonPressEnum.Alarm, fnItems);
        }

        private void ProcessStates(string[] items, TransferData.CmdItems cmdItems)
        {
            ProcessState(items[10], TransferData.ButtonPressEnum.GSM, cmdItems);
            ProcessState(items[11], TransferData.ButtonPressEnum.WIFI, cmdItems);
            ProcessState(items[12], TransferData.ButtonPressEnum.AlarmOff, cmdItems);
        }

        private void ProcessStates<T>(int startPos, string[] items, TransferData.PanelItems<T> panelItems) where T: Enum
        {
            var enumList = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
            for (int i = 0; i < 12; i++)
            {
                if (byte.TryParse(items[startPos + i], out byte iState))
                {
                    panelItems.SetSate(enumList[i], (TransferData.FnStateEnum)iState);
                }
            }
        }

        private void ProcessState(string value, TransferData.ButtonPressEnum buttonPress, TransferData.FnItems fnItems)
        {
            if (byte.TryParse(value, out byte iState))
            {
                fnItems.SetState(buttonPress, (TransferData.FnStateEnum)iState);
            }
        }

        private void ProcessState(string value, TransferData.ButtonPressEnum buttonPress, TransferData.CmdItems cmdItems)
        {
            if (byte.TryParse(value, out byte iState))
            {
                cmdItems.SetPressed(buttonPress, iState == 1);
            }
        }

        private string GetToken()
        {
            return Crpt.Crpt.Crypt(DateTime.UtcNow.AddMinutes(1).ToString(), _TOKEN_PASSWORD);
        }

        private bool CheckToken(string token)
        {
            var dateStr = Crpt.Crpt.Decrypt(token, _TOKEN_PASSWORD);
            if (!DateTime.TryParse(dateStr, out DateTime date))
                return false;
            if (date < DateTime.UtcNow)
                return false;
            return true;
        }

        private bool CheckSecondsToken(string seconds)
        {
            if (!int.TryParse(seconds, out int iSeconds))
                return false;
            var nowSeconds = _dateHelper.GetSeconds();
            if ((nowSeconds - iSeconds) > 360)
                return false;
            return true;
        }
    }
}
