using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raspberry.IO.Components.Sensors.Compass.Hmc5883
{
    public enum Hmc5883Mode : byte
    {
        Normal = 0x00,
        Positive = 0x01,
        Negative = 0x02,

        Default = Normal
    }
}
