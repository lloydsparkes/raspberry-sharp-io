using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raspberry.IO.Components.Sensors.Accelerometer.Adxl345
{
    /// <summary>
    /// Represents the rate at which measurements are taken
    /// </summary>
    public enum Adxl345DataRate : byte
    {
        R0_10Hz = 0x00,
        R0_20Hz = 0x01,
        R0_39Hz = 0x02,
        R0_78Hz = 0x03,
        R1_56Hz = 0x04,
        R3_13Hz = 0x05,
        R6_25Hz = 0x06,
        R12_5Hz = 0x07,
        R25Hz   = 0x08,
        R50Hz   = 0x09,
        R100Hz  = 0x0A,
        R200Hz  = 0x0B,
        R400Hz  = 0x0C,
        R800Hz  = 0x0D,
        R1600Hz = 0x0E,
        R3200Hz = 0x0F,

        Default = R100Hz
    }
}
