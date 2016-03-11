using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raspberry.IO.Components.Sensors.Compass.Hmc5883
{
    public class Hmc5883Data
    {
        public double X;
        public double Y;
        public double Z;

        /// <summary>
        /// Turn the basic values into a Bearing - assuming the sensor is level
        /// </summary>
        public double Bearing
        {
            get
            {
                var bearing = Math.Atan2(Y, X);
                if(bearing < 0)
                {
                    bearing = bearing + (Math.PI * 2);
                }
                return bearing;
            }
        }

        public double CompensatedBearing(double pitch, double roll)
        {
            var cosPitch = Math.Cos(pitch);
            var sinPitch = Math.Sin(pitch);

            var cosRoll = Math.Cos(roll);
            var sinRoll = Math.Sin(roll);

            var Xh = (X * cosRoll) + (Z * sinRoll);
            var Yh = (X * sinPitch * sinRoll) + (Y * cosPitch) - (Z * sinPitch * cosRoll);

            var bearing = Math.Atan2(Yh, Xh);
            if (bearing < 0)
            {
                bearing = bearing + (Math.PI * 2);
            }
            return bearing;
        }
    }
}
