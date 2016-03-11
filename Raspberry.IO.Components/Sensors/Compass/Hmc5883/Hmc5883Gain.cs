using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raspberry.IO.Components.Sensors.Compass.Hmc5883
{
    public enum Hmc5883Gain : byte
    {
        GA0_88 = 0x00,
        GA1_30 = 0x20,
        GA1_90 = 0x40,
        GA2_50 = 0x60,
        GA4_00 = 0x80,
        GA4_70 = 0xA0,
        GA5_60 = 0xC0,
        GA8_10 = 0xE0,

        Default = GA1_30
    }
}
