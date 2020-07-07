using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.ClientService.Models
{
    public class Hotkey
    {
        public string hotkey { get; set; }
        public string HotkeyDesc { get; set; }
        public string SampleMsg { get; set; }
        public string PhysicalReg { get; set; }
        public string TransBlockTimeStatus { get; set; }
        public string Example { get; set; }
        public string FuncProcName { get; set; }
        public string Status { get; set; }
        public int? PinPosition { get; set; }
        public int? ToPhonePosition { get; set; }
    }
}
