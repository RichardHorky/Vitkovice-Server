﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
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
        private readonly SyncLog _syncLog;
        private readonly ILogger<SyncController> _logger;
        private readonly GlobalData _globalData;
        private const string _TOKEN_PASSWORD = "x4tr5Gj";
        private const string _INVALID_TOKEN = "invalid_token";

        public SyncController(Helpers.DateHelper dateHelper, DataStorage dataStorage, ChangeNotifier changeNotifier, Errors errors, SyncLog syncLog,
            ILogger<SyncController> logger, GlobalData globalData)
        {
            _dateHelper = dateHelper;
            _dataStorage = dataStorage;
            _changeNotifier = changeNotifier;
            _errors = errors;
            _syncLog = syncLog;
            _logger = logger;
            _globalData = globalData;
        }

        public ActionResult<string> Get()
        {
            _logger.LogDebug("Get");
            try
            {
                _globalData.LastArduinoRequest = DateTime.Now;

                var seconds = _dateHelper.GetSeconds();
                var fnItems = _dataStorage.GetData<Data.TransferData.FnItems>(out bool fileExists);
                var itemsList = new List<string>()
                {
                    GetToken(),
                    seconds.ToString(),
                    //fnItems.ID
                    ((fnItems?.Source ?? TransferData.SourceEnum.None) == TransferData.SourceEnum.Server && (fnItems?.Valid ?? false) ? fnItems.ID : "NONE")
                };
                AddItemToList(itemsList, Data.TransferData.ButtonPressEnum.Termostat1, fnItems);
                AddItemToList(itemsList, Data.TransferData.ButtonPressEnum.Termostat2, fnItems);
                itemsList.Add(fnItems.GetState(TransferData.ButtonPressEnum.TermostatR1) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(fnItems.GetState(TransferData.ButtonPressEnum.TermostatR2) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(fnItems.GetState(TransferData.ButtonPressEnum.TermostatR3) == TransferData.FnStateEnum.Off ? "0" : "1");
                AddItemToList(itemsList, Data.TransferData.ButtonPressEnum.ElHeating, fnItems);
                AddItemToList(itemsList, Data.TransferData.ButtonPressEnum.Water, fnItems);
                AddItemToList(itemsList, Data.TransferData.ButtonPressEnum.Cams, fnItems);
                AddItemToList(itemsList, Data.TransferData.ButtonPressEnum.Alarm, fnItems);

                var cmdItems = _dataStorage.GetData<TransferData.CmdItems>(out fileExists);
                itemsList.Add((cmdItems?.Source ?? TransferData.SourceEnum.None) == TransferData.SourceEnum.Server && (cmdItems?.Valid ?? false) ? cmdItems.ID : string.Empty);
                AddCmdItemToList(itemsList, TransferData.ButtonPressEnum.GSM, cmdItems);
                AddCmdItemToList(itemsList, TransferData.ButtonPressEnum.WIFI, cmdItems);
                AddCmdItemToList(itemsList, TransferData.ButtonPressEnum.AlarmOff, cmdItems);

                var resStr = string.Join('|', itemsList);
                _logger.LogDebug($"Get; result: {resStr}");

                return Ok($"{{{resStr}}}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _errors.ErrorList.Add(new ErrorModel(ex.ToString()));
            }
            return Ok();
        }

        [HttpGet]
        [Route("Post/{data}")]
        public ActionResult Post(string data)
        {
            _logger.LogDebug($"POST; data: {data}");
            try
            {
                _globalData.LastArduinoRequest = DateTime.Now;

                var list = data.Split('|');
                if (list.Length == 0 || !CheckToken(list[0]))
                    return Ok();

                var fnItems = _dataStorage.GetData<TransferData.FnItems>(out bool fileExists) ?? new TransferData.FnItems();
                var validWaiting = fnItems.Source == TransferData.SourceEnum.Server && fnItems.Valid && list[2] != fnItems.ID;
                //is valid waiting - skip it
                if (!validWaiting)
                {
                    //prevent loosing of rooms terms info
                    var tR1Handle = fnItems.GetState(TransferData.ButtonPressEnum.TermostatR1);
                    var tR2Handle = fnItems.GetState(TransferData.ButtonPressEnum.TermostatR2);
                    var tR3Handle = fnItems.GetState(TransferData.ButtonPressEnum.TermostatR3);
                    fnItems = new TransferData.FnItems();
                    //pool has not been cleared
                    if (fileExists)
                    {
                        fnItems.SetState(TransferData.ButtonPressEnum.TermostatR1, tR1Handle);
                        fnItems.SetState(TransferData.ButtonPressEnum.TermostatR2, tR2Handle);
                        fnItems.SetState(TransferData.ButtonPressEnum.TermostatR3, tR3Handle);
                    }
                    ProcessStates(list, fnItems, fileExists);
                    fnItems.Source = TransferData.SourceEnum.Arduino;
                    fnItems.Date = DateTime.UtcNow;
                    _dataStorage.SaveData(fnItems);
                    //save to recovery after terminal has lost power
                    _dataStorage.SaveData(fnItems, "FnRecovery");
                }

                var cmdItems = _dataStorage.GetData<TransferData.CmdItems>(out fileExists) ?? new TransferData.CmdItems();
                validWaiting = cmdItems.Source == TransferData.SourceEnum.Server && cmdItems.Valid && list[12] != cmdItems.ID;
                //is valid waiting - skip it
                if (!validWaiting)
                {
                    cmdItems = new TransferData.CmdItems();
                    ProcessStates(list, cmdItems);
                    cmdItems.Source = TransferData.SourceEnum.Arduino;
                    cmdItems.Date = DateTime.UtcNow;
                    _dataStorage.SaveData(cmdItems);
                }

                var inputItems = new TransferData.PanelItems<TransferData.InputStatusEnum>();
                ProcessStates(16, list, inputItems);
                _dataStorage.SaveData(inputItems, "InputItems");

                var outputItems = new TransferData.PanelItems<TransferData.OutputStatusEnum>();
                ProcessStates(31, list, outputItems);
                _dataStorage.SaveData(outputItems, "OutputItems");

                _syncLog.Log(inputItems, outputItems);

                var args = new DataChangedArgs() { FnItems = fnItems, CmdItems = cmdItems, InputItems = inputItems, OutputItems = outputItems };
                _changeNotifier.OnNotify(args);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _errors.ErrorList.Add(new ErrorModel(ex.ToString()));
            }

            return Ok();
        }

        [HttpGet]
        [Route("Recovery")]
        public ActionResult<string> Recovery()
        {
            _logger.LogDebug("Recovery");
            try
            {
                _globalData.LastArduinoRequest = DateTime.Now;

                var fnRecovery = _dataStorage.GetData<TransferData.FnItems>(out bool fileExists, "FnRecovery");
                if (fnRecovery == null)
                    return Ok("{1|1|0|0|0|1|1|1|1}");

                var items = new List<string>();
                items.Add(((byte)fnRecovery.GetState(TransferData.ButtonPressEnum.Termostat1)).ToString());
                items.Add(((byte)fnRecovery.GetState(TransferData.ButtonPressEnum.Termostat2)).ToString());
                items.Add(((byte)fnRecovery.GetState(TransferData.ButtonPressEnum.TermostatR1)).ToString());
                items.Add(((byte)fnRecovery.GetState(TransferData.ButtonPressEnum.TermostatR2)).ToString());
                items.Add(((byte)fnRecovery.GetState(TransferData.ButtonPressEnum.TermostatR3)).ToString());
                items.Add(((byte)fnRecovery.GetState(TransferData.ButtonPressEnum.ElHeating)).ToString());
                items.Add(((byte)fnRecovery.GetState(TransferData.ButtonPressEnum.Water)).ToString());
                items.Add(((byte)fnRecovery.GetState(TransferData.ButtonPressEnum.Cams)).ToString());
                items.Add(((byte)fnRecovery.GetState(TransferData.ButtonPressEnum.Alarm)).ToString());

                var resStr = string.Join('|', items);
                _logger.LogDebug($"Recovery; result: {resStr}");

                return Ok($"{{{resStr}}}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
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

        private void ProcessStates(string[] items, TransferData.FnItems fnItems, bool fileEdists)
        {
            ProcessState(items[3], TransferData.ButtonPressEnum.Termostat1, fnItems);
            ProcessState(items[4], TransferData.ButtonPressEnum.Termostat2, fnItems);
            if (!fileEdists)
            {
                ProcessState(items[5], TransferData.ButtonPressEnum.TermostatR1, fnItems);
                ProcessState(items[6], TransferData.ButtonPressEnum.TermostatR2, fnItems);
                ProcessState(items[7], TransferData.ButtonPressEnum.TermostatR3, fnItems);
            }
            ProcessState(items[8], TransferData.ButtonPressEnum.ElHeating, fnItems);
            ProcessState(items[9], TransferData.ButtonPressEnum.Water, fnItems);
            ProcessState(items[10], TransferData.ButtonPressEnum.Cams, fnItems);
            ProcessState(items[11], TransferData.ButtonPressEnum.Alarm, fnItems);
        }

        private void ProcessStates(string[] items, TransferData.CmdItems cmdItems)
        {
            ProcessState(items[13], TransferData.ButtonPressEnum.GSM, cmdItems);
            ProcessState(items[14], TransferData.ButtonPressEnum.WIFI, cmdItems);
            ProcessState(items[15], TransferData.ButtonPressEnum.AlarmOff, cmdItems);
        }

        private void ProcessStates<T>(int startPos, string[] items, TransferData.PanelItems<T> panelItems) where T: Enum
        {
            var enumList = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
            for (int i = 0; i < enumList.Length; i++)
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
            return true;
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
