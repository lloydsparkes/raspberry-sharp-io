using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raspberry.IO.Components.Sensors.Compass.Hmc5883
{
    public enum Hmc5883SampleRate : byte
    {
        S1 = 0x00,
        S2 = 0x20,
        S4 = 0x40,
        S8 = 0x60,

        Default = S8
    }
}
