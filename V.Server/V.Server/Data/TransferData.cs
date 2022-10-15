using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V.Server.Data
{
    public class TransferData
    {
        public enum InputStatusEnum : short
        {
            FireplacePump = 0x1,        //1
            FireplaceAkum = 0x2,        //2
            Termostat1 = 0x4,           //4
            Termostat2 = 0x8,           //8
            TermostatR1 = 0x10,         //16
            TermostatR2 = 0x20,         //32
            TermostatR3 = 0x40,         //64
            DiffTerm = 0x80,            //128
            PipeTerm = 0x100,           //256
            SMSWater = 0x200,           //512
            AlarmKey = 0x400,           //1024
            AlarmRound1 = 0x800,        //2048
            AlarmRound2 = 0x1000,       //4096
            Valv1Status = 0x2000,       //8192
            Valv2Status = 0x4000        //16384
        }

        public enum OutputStatusEnum
        {
            ElHeating = 0x1,            //1
            ElHeatingPump = 0x2,        //2
            PumpAku = 0x4,              //4
            WifiReset = 0x8,            //8
            Zone = 0x10,                //16
            Water = 0x20,               //32
            Cams = 0x40,                //64
            Alarm = 0x80,               //128
            ResetGSM = 0x100,           //256
            Valv1 = 0x200,              //512
            Valv2 = 0x400,              //1024
            Valv3 = 0x800               //2048
        }

        public enum ButtonPressEnum : short
        {
            GSM = 0x1,                  //1
            WIFI = 0x2,                 //2
            AlarmOff = 0x4,             //4
            Termostat1 = 0x8,           //8
            Termostat2 = 0x10,          //16
            TermostatR1 = 0x20,         //32
            TermostatR2 = 0x40,         //64
            TermostatR3 = 0x80,         //128
            ElHeating = 0x100,          //256
            Water = 0x200,              //512
            Cams = 0x400,               //1024
            Alarm = 0x800               //2048
        }

        public enum FnStateEnum : byte
        {
            Off = 0,
            Auto = 1,
            On = 2
        }

        public enum SourceEnum : byte
        {
            Server = 0,
            Arduino = 1,
            None = 255
        }

        public class FnItem
        {
            public FnItem()
            {
                FnState = FnStateEnum.Auto;
            }
            public ButtonPressEnum ButtonStatus { get; set; }
            public FnStateEnum FnState { get; set; }
            public void SwitchState()
            {
                var state = (byte)FnState;
                state++;
                if (state > 2)
                    state = 0;
                FnState = (FnStateEnum)state;
            }
        }

        public class FnItems : TransferDataBase
        {
            private const int _EXP_SECONDS = 180;
            public void Reset()
            {
                Date = DateTime.MinValue;
                foreach (var item in Items)
                    item.FnState = FnStateEnum.Auto;
                SetState(ButtonPressEnum.TermostatR1, FnStateEnum.Off);
                SetState(ButtonPressEnum.TermostatR2, FnStateEnum.Off);
                SetState(ButtonPressEnum.TermostatR3, FnStateEnum.Off);
            }
            public List<FnItem> Items { get; set; } = new List<FnItem>();
            public void SwitchItem(ButtonPressEnum buttonPress)
            {
                var item = Items.Where(i => i.ButtonStatus == buttonPress).FirstOrDefault();
                if (item == null)
                {
                    item = new FnItem() { ButtonStatus = buttonPress };
                    Items.Add(item);
                }
                item.SwitchState();
                Date = DateTime.UtcNow;
                Source = SourceEnum.Server;
            }
            public FnStateEnum GetState(ButtonPressEnum buttonPress)
            {
                var item = Items.Where(i => i.ButtonStatus == buttonPress).FirstOrDefault();
                if (item == null)
                    return FnStateEnum.Auto;
                return item.FnState;
            }
            public void SetState(ButtonPressEnum buttonPress, FnStateEnum fnState, bool sendToClient = false)
            {
                var item = Items.Where(i => i.ButtonStatus == buttonPress).FirstOrDefault();
                if (item == null)
                {
                    item = new FnItem() { ButtonStatus = buttonPress };
                    Items.Add(item);
                }
                item.FnState = fnState;
                if (sendToClient)
                {
                    Date = DateTime.UtcNow;
                    Source = SourceEnum.Server;
                }
            }
            public bool Valid => Source == SourceEnum.Server && (DateTime.UtcNow - Date).TotalSeconds <= _EXP_SECONDS;
        }

        public class CmdItem
        {
            public ButtonPressEnum ButtonStatus { get; set; }
            public bool Pressed { get; set; }
        }

        public class CmdItems : TransferDataBase
        {
            private const int _EXP_SECONDS = 180;
            public void Reset()
            {
                Date = DateTime.MinValue;
                foreach (var item in Items)
                    item.Pressed = false;
            }
            public List<CmdItem> Items { get; set; } = new List<CmdItem>();
            public void SetPressed(ButtonPressEnum buttonPress, bool pressed)
            {
                var item = Items.Where(i => i.ButtonStatus == buttonPress).FirstOrDefault();
                if (item == null)
                {
                    item = new CmdItem() { ButtonStatus = buttonPress };
                    Items.Add(item);
                }
                item.Pressed = pressed;
                Date = DateTime.UtcNow;
                Source = SourceEnum.Server;
            }
            public bool GetPressed(ButtonPressEnum buttonPress)
            {
                var item = Items.Where(i => i.ButtonStatus == buttonPress).FirstOrDefault();
                return item?.Pressed ?? false;
            }
            public bool Valid => Source == SourceEnum.Server && (DateTime.UtcNow - Date).TotalSeconds <= _EXP_SECONDS;
        }

        public class PanelItems<T> where T : Enum
        {
            public List<PanelItem<T>> Items { get; set; } = new List<PanelItem<T>>();
            public void SetSate(T itemMember, FnStateEnum status)
            {
                var item = Items.Where(i => i.ItemMember.Equals(itemMember)).FirstOrDefault();
                if (item == null)
                {
                    item = new PanelItem<T>() { ItemMember = itemMember };
                    Items.Add(item);
                }
                item.FnState = status;
            }
            public FnStateEnum GetState(T itemMember)
            {
                var item = Items.Where(i => i.ItemMember.Equals(itemMember)).FirstOrDefault();
                return item?.FnState ?? FnStateEnum.Off;
            }
        }

        public class PanelItem<T> where T: Enum
        {
            public T ItemMember { get; set; }
            public FnStateEnum FnState { get; set; }
        }

        public class TransferDataBase
        {
            public TransferDataBase()
            {
                GenerateID();
            }
            public SourceEnum Source { get; set; }
            public DateTime Date { get; set; }
            public string ID { get; set; }
            private void GenerateID()
            {
                ID = Guid.NewGuid().ToString();
            }
        }
    }
}
