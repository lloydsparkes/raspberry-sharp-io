using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raspberry.IO.Components.Sensors.Compass.Hmc5883
{
    public enum Hmc5883MeasurementMode : byte
    {
        Continous = 0x00,
        Single = 0x01,
        Idle = 0x02,

        Default = Single
    }
}
