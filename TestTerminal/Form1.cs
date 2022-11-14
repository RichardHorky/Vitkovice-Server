using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;

using V.Server.Data;
using static System.Net.WebRequestMethods;

namespace TestTerminal
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _client;
        private const string _BASE_ADDRESS = "https://localhost:5001/";
        private const int _SYNC_SPAN = 20;
        private const int _DISP_TIME_SPAN = 10;
        private bool _divider;
        private string _lastFnItemsID;
        private string _lastCmdItemsID;
        private DateTime _gsmPressed = DateTime.MinValue;
        private DateTime _wifiPressed = DateTime.MinValue;
        private DateTime _alarmPressed = DateTime.MinValue;

        TransferData.FnItems _fnItems = new TransferData.FnItems();
        TransferData.CmdItems _cmdItems = new TransferData.CmdItems();
        TransferData.PanelItems<TransferData.InputStatusEnum> _inputItems = new TransferData.PanelItems<TransferData.InputStatusEnum>();
        TransferData.PanelItems<TransferData.OutputStatusEnum> _outputItems = new TransferData.PanelItems<TransferData.OutputStatusEnum>();

        public Form1()
        {
            InitializeComponent();

            _client = new HttpClient() { BaseAddress = new Uri(_BASE_ADDRESS) };
        }


        private int _syncCounter;
        private async void timer_Tick(object sender, EventArgs e)
        {
            if (chAutoSync.Checked)
            {
                _syncCounter++;
                if (_syncCounter > _SYNC_SPAN)
                {
                    _syncCounter = 0;
                    await DoSync();
                }
            }

            _divider = !_divider;
            DispTime();
            DispStatuses();

            if ((DateTime.Now - _gsmPressed).TotalSeconds > 30)
                _cmdItems.SetPressed(TransferData.ButtonPressEnum.GSM, false);
            if ((DateTime.Now - _wifiPressed).TotalSeconds > 30)
                _cmdItems.SetPressed(TransferData.ButtonPressEnum.WIFI, false);
            if ((DateTime.Now - _alarmPressed).TotalSeconds > 30)
                _cmdItems.SetPressed(TransferData.ButtonPressEnum.AlarmOff, false);
        }

        private string _token;
        private bool _inError;
        private async Task DoSync()
        {
            _inError = false;
            try
            {
                var itemsList = new List<string>()
            {
                _token, string.Empty, _lastFnItemsID
            };
                AddItemToList(itemsList, TransferData.ButtonPressEnum.Termostat1, _fnItems);
                AddItemToList(itemsList, TransferData.ButtonPressEnum.Termostat2, _fnItems);
                AddItemToList(itemsList, TransferData.ButtonPressEnum.TermostatR1, _fnItems);
                AddItemToList(itemsList, TransferData.ButtonPressEnum.TermostatR2, _fnItems);
                AddItemToList(itemsList, TransferData.ButtonPressEnum.TermostatR3, _fnItems);
                AddItemToList(itemsList, TransferData.ButtonPressEnum.ElHeating, _fnItems);
                AddItemToList(itemsList, TransferData.ButtonPressEnum.Water, _fnItems);
                AddItemToList(itemsList, TransferData.ButtonPressEnum.Cams, _fnItems);
                AddItemToList(itemsList, TransferData.ButtonPressEnum.Alarm, _fnItems);

                itemsList.Add(_lastCmdItemsID);
                AddItemToList(itemsList, TransferData.ButtonPressEnum.GSM, _cmdItems);
                AddItemToList(itemsList, TransferData.ButtonPressEnum.WIFI, _cmdItems);
                AddItemToList(itemsList, TransferData.ButtonPressEnum.AlarmOff, _cmdItems);

                itemsList.Add(_inputItems.GetState(TransferData.InputStatusEnum.FireplacePump) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_inputItems.GetState(TransferData.InputStatusEnum.FireplaceAkum) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_inputItems.GetState(TransferData.InputStatusEnum.Termostat1) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_inputItems.GetState(TransferData.InputStatusEnum.Termostat2) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_inputItems.GetState(TransferData.InputStatusEnum.TermostatR1) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_inputItems.GetState(TransferData.InputStatusEnum.TermostatR2) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_inputItems.GetState(TransferData.InputStatusEnum.TermostatR3) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_inputItems.GetState(TransferData.InputStatusEnum.DiffTerm) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_inputItems.GetState(TransferData.InputStatusEnum.PipeTerm) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_inputItems.GetState(TransferData.InputStatusEnum.SMSWater) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_inputItems.GetState(TransferData.InputStatusEnum.AlarmKey) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_inputItems.GetState(TransferData.InputStatusEnum.AlarmRound1) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_inputItems.GetState(TransferData.InputStatusEnum.AlarmRound2) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_inputItems.GetState(TransferData.InputStatusEnum.Valv1Status) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_inputItems.GetState(TransferData.InputStatusEnum.Valv2Status) == TransferData.FnStateEnum.Off ? "0" : "1");

                itemsList.Add(_outputItems.GetState(TransferData.OutputStatusEnum.ElHeating) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_outputItems.GetState(TransferData.OutputStatusEnum.ElHeatingPump) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_outputItems.GetState(TransferData.OutputStatusEnum.PumpAku) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_outputItems.GetState(TransferData.OutputStatusEnum.WifiReset) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_outputItems.GetState(TransferData.OutputStatusEnum.Zone) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_outputItems.GetState(TransferData.OutputStatusEnum.Water) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_outputItems.GetState(TransferData.OutputStatusEnum.Cams) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_outputItems.GetState(TransferData.OutputStatusEnum.Alarm) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_outputItems.GetState(TransferData.OutputStatusEnum.ResetGSM) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_outputItems.GetState(TransferData.OutputStatusEnum.Valv1) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_outputItems.GetState(TransferData.OutputStatusEnum.Valv2) == TransferData.FnStateEnum.Off ? "0" : "1");
                itemsList.Add(_outputItems.GetState(TransferData.OutputStatusEnum.Valv3) == TransferData.FnStateEnum.Off ? "0" : "1");

                var dataStr = string.Join("|", itemsList);

                await _client.GetStringAsync($"api/sync/post/{dataStr}");
                _lastFnItemsID = null;

                var result = await _client.GetStringAsync("api/sync");
                result = result.Replace("{", "").Replace("}", "");
                var list = result.Split('|');

                if (int.TryParse(list[1], out int baseSeconds))
                    DispTime(baseSeconds);

                _token = list[0];
                _lastFnItemsID = list[2];
                _lastCmdItemsID = list[9];
                ProcessStates(list);
            }
            catch (Exception ex)
            {
                lbDisplay.Text = ex.Message;
                _inError = true;
            }
        }

        private void AddItemToList(List<string> list, TransferData.ButtonPressEnum buttonPress, TransferData.FnItems fnItems)
        {
            list.Add(((byte)fnItems.GetState(buttonPress)).ToString());
        }

        private void AddItemToList(List<string> list, TransferData.ButtonPressEnum buttonPress, TransferData.CmdItems cmdItems)
        {
            list.Add(cmdItems.GetPressed(buttonPress) ? "1" : "0");
        }

        private int _baseSeconds;
        private DateTime _lastSync;
        private int _dispTimeCounter;
        private void DispTime(int baseSeconds)
        {
            _dispTimeCounter = 0;
            _baseSeconds = baseSeconds;
            _lastSync = DateTime.Now;
        }

        private void DispTime()
        {
            _dispTimeCounter++;
            if (_inError)
                return;
            if (_dispTimeCounter > _DISP_TIME_SPAN)
            {
                lbDisplay.Text = null;
                return;
            }

            var nowSeconds = _baseSeconds + (int)(DateTime.Now - _lastSync).TotalSeconds;
            long daySeconds = nowSeconds % 86400;
            int hours = (int)(daySeconds / 3600);
            long restSeconds = daySeconds % 3600;
            int minutes = (int)(restSeconds / 60);
            long seconds = restSeconds % 60;

            var hStr = hours.ToString();
            if (hours < 10)
                hStr = "0" + hStr;
            var mStr = minutes.ToString();
            if (minutes < 10)
                mStr = "0" + mStr;
            var sStr = seconds.ToString();
            if (seconds < 10)
                sStr = "0" + sStr;

            lbDisplay.Text = $"Synchronizace se serverem\nAktualni cas: {hStr}:{mStr}:{sStr}";
        }

        private void DispStatuses()
        {
            foreach (var item in _fnItems.Items)
                ShowFnStatus(item.FnState, item.ButtonStatus);
        }

        private void ShowFnStatus(TransferData.FnStateEnum fnState, TransferData.ButtonPressEnum buttonStatus)
        {
            Panel led;
            switch (buttonStatus)
            {
                case TransferData.ButtonPressEnum.Alarm:
                    led = ledAlarm;
                    break;
                case TransferData.ButtonPressEnum.Cams:
                    led = ledCam;
                    break;
                case TransferData.ButtonPressEnum.Termostat1:
                    led = ledTerm1;
                    break;
                case TransferData.ButtonPressEnum.Termostat2:
                    led = ledTerm2;
                    break;
                case TransferData.ButtonPressEnum.TermostatR1:
                    chReadTR1.Checked = fnState == TransferData.FnStateEnum.Auto;
                    return;
                case TransferData.ButtonPressEnum.TermostatR2:
                    chReadTR2.Checked = fnState == TransferData.FnStateEnum.Auto;
                    return;
                case TransferData.ButtonPressEnum.TermostatR3:
                    chReadTR3.Checked = fnState == TransferData.FnStateEnum.Auto;
                    return;
                case TransferData.ButtonPressEnum.Water:
                    led = ledWater;
                    break;
                case TransferData.ButtonPressEnum.ElHeating:
                    led = ledEkotel;
                    break;
                default:
                    throw new NotImplementedException();
            }
            led.BackColor = fnState == TransferData.FnStateEnum.Auto || (fnState == TransferData.FnStateEnum.On && _divider) ?
                Color.LimeGreen :
                Color.Gray;
        }

        private void btn_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            TransferData.ButtonPressEnum? btnPress = null;
            if (button == btnAlarmFn)
                btnPress = TransferData.ButtonPressEnum.Alarm;
            if (button == btnCamFn)
                btnPress = TransferData.ButtonPressEnum.Cams;
            if (button == btnWaterFn)
                btnPress = TransferData.ButtonPressEnum.Water;
            if (button == btnEkotelFn)
                btnPress = TransferData.ButtonPressEnum.ElHeating;
            if (button == btnTerm2Fn)
                btnPress = TransferData.ButtonPressEnum.Termostat2;
            if (button == btnTerm1Fn)
                btnPress = TransferData.ButtonPressEnum.Termostat1;

            if (btnPress.HasValue)
                _fnItems.SwitchItem(btnPress.Value);

            if (button == btnGSM)
            {
                _gsmPressed = DateTime.Now;
                _cmdItems.SetPressed(TransferData.ButtonPressEnum.GSM, true);
            }
            if (button == btnWifi)
            {
                _wifiPressed = DateTime.Now;
                _cmdItems.SetPressed(TransferData.ButtonPressEnum.WIFI, true);
            }
            if (button == btnAlarm)
            {
                _alarmPressed = DateTime.Now;
                _cmdItems.SetPressed(TransferData.ButtonPressEnum.AlarmOff, true);
            }
        }

        private void ProcessStates(string[] items)
        {
            ProcessState(items[3], TransferData.ButtonPressEnum.Termostat1);
            ProcessState(items[4], TransferData.ButtonPressEnum.Termostat2);
            ProcessState(items[5], TransferData.ButtonPressEnum.TermostatR1);
            ProcessState(items[6], TransferData.ButtonPressEnum.TermostatR2);
            ProcessState(items[7], TransferData.ButtonPressEnum.TermostatR3);
            ProcessState(items[8], TransferData.ButtonPressEnum.ElHeating);
            ProcessState(items[9], TransferData.ButtonPressEnum.Water);
            ProcessState(items[10], TransferData.ButtonPressEnum.Cams);
            ProcessState(items[11], TransferData.ButtonPressEnum.Alarm);

            ProcessCmdState(items[12], TransferData.ButtonPressEnum.GSM);
            ProcessCmdState(items[13], TransferData.ButtonPressEnum.WIFI);
            ProcessCmdState(items[14], TransferData.ButtonPressEnum.AlarmOff);
        }

        private void ProcessState(string value, TransferData.ButtonPressEnum buttonPress)
        {
            if (byte.TryParse(value, out byte iState))
            {
                _fnItems.SetState(buttonPress, (TransferData.FnStateEnum)iState);
            }
        }

        private void ProcessCmdState(string value, TransferData.ButtonPressEnum buttonPress)
        {
            if (byte.TryParse(value, out byte iState))
            {
                _cmdItems.SetPressed(buttonPress, iState == 1);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await DoSync();
        }
    }
}
