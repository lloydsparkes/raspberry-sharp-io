using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raspberry.IO.Components.Sensors.Gyro.L3g4200d
{
    /// <summary>
    /// Represents the data returned from <see cref="L3g4200dConnection"/>
    /// </summary>
    public class L3g4200dData
    {
        /// <summary>
        /// Angular Velocity - X radians/second
        /// </summary>
        public double X;

        /// <summary>
        /// Angular Velocity - Y radians/second
        /// </summary>
        public double Y;

        /// <summary>
        /// Angular Velocity - Z radians/second
        /// </summary>
        public double Z;
    }
}
