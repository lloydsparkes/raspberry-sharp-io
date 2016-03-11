using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raspberry.IO.Components.Sensors.Accelerometer.Adxl345
{
    /// <summary>
    /// Represents data from a <see cref="Adxl345Connection"/>
    /// </summary>
    public class Adxl345Data
    {
        public UnitsNet.Acceleration X;
        public UnitsNet.Acceleration Y;
        public UnitsNet.Acceleration Z;
    }
}
