using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Common.Models.Device
{

    public enum Power
    {
        On,
        Off
    }

    public class Led : Device
    {
       
        public string Power { get;  set; }
    }
}
