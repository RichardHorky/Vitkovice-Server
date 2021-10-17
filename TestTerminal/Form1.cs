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

namespace TestTerminal
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _client;
        private const string _BASE_ADDRESS = "http://192.168.2.6:5000/";
        private const int _SYNC_SPAN = 20;
        private const int _DISP_TIME_SPAN = 10;
        private bool _divider;
        private string _lastFnItemsID;

        TransferData.FnItems _fnItems = new TransferData.FnItems();
        TransferData.CmdItems _cmdItems = new TransferData.CmdItems();

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
        }

        private string _token;
        private async Task DoSync()
        {
            var fnItemsList = new List<string>()
            {
                _token, string.Empty, _lastFnItemsID
            };
            AddItemToList(fnItemsList, TransferData.ButtonPressEnum.Termostat1, _fnItems);
            AddItemToList(fnItemsList, TransferData.ButtonPressEnum.Termostat2, _fnItems);
            AddItemToList(fnItemsList, TransferData.ButtonPressEnum.ElHeating, _fnItems);
            AddItemToList(fnItemsList, TransferData.ButtonPressEnum.Water, _fnItems);
            AddItemToList(fnItemsList, TransferData.ButtonPressEnum.Cams, _fnItems);
            AddItemToList(fnItemsList, TransferData.ButtonPressEnum.Alarm, _fnItems);
            var dataStr = string.Join("|", fnItemsList);

            await _client.GetStringAsync($"api/sync/post/{dataStr}");
            _lastFnItemsID = null;

            var result = await _client.GetStringAsync("api/sync");
            result = result.Replace("{", "").Replace("}", "");
            var list = result.Split('|');

            if (int.TryParse(list[1], out int baseSeconds))
                DispTime(baseSeconds);

            _token = list[0];
            _lastFnItemsID = list[2];
            ProcessStates(list);
        }

        private void AddItemToList(List<string> list, TransferData.ButtonPressEnum buttonPress, TransferData.FnItems fnItems)
        {
            list.Add(((byte)fnItems.GetState(buttonPress)).ToString());
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
        }

        private void ProcessStates(string[] items)
        {
            ProcessState(items[3], TransferData.ButtonPressEnum.Termostat1);
            ProcessState(items[4], TransferData.ButtonPressEnum.Termostat2);
            ProcessState(items[5], TransferData.ButtonPressEnum.ElHeating);
            ProcessState(items[6], TransferData.ButtonPressEnum.Water);
            ProcessState(items[7], TransferData.ButtonPressEnum.Cams);
            ProcessState(items[8], TransferData.ButtonPressEnum.Alarm);
        }

        private void ProcessState(string value, TransferData.ButtonPressEnum buttonPress)
        {
            if (byte.TryParse(value, out byte iState))
            {
                _fnItems.SetState(buttonPress, (TransferData.FnStateEnum)iState);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await DoSync();
        }
    }
}
