using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raspberry.IO.Components.Sensors.Gyro.L3g4200d
{
    public enum L3g4200dScale : byte
    {
        S250 = 0x00,
        S500 = 0x10,
        S2000 = 0x30,

        Default = S250
    }
}
