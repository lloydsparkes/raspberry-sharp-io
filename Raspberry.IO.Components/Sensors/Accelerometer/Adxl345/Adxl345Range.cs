using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raspberry.IO.Components.Sensors.Accelerometer.Adxl345
{
    /// <summary>
    /// Represents the Measurement Range +/- Range (2,4,8,16)G
    /// </summary>
    public enum Adxl345Range : byte
    {
        R2G = 0x00, 
        R4G = 0x01,
        R8G = 0x02,
        R16G = 0x03,

        Default = R2G
    }
}
