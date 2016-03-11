using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raspberry.IO.Components.Sensors.Compass.Hmc5883
{
    public enum Hmc5883DataRate : byte
    {
        R0_75Hz = 0x00,
        R1_50Hz = 0x04,
        R3_00Hz = 0x08,
        R7_50Hz = 0x0C,
        R15_0Hz = 0x10,
        R30_0Hz = 0x14,
        R75_0Hz = 0x18,

        Default = R15_0Hz
    }
}
