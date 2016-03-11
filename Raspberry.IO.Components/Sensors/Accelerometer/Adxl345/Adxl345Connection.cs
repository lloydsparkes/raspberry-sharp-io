using Raspberry.IO.InterIntegratedCircuit;
using System;

namespace Raspberry.IO.Components.Sensors.Accelerometer.Adxl345
{
    public class Adxl345Connection
    {
        #region Fields

        private readonly I2cDeviceConnection connection;
        private Adxl345DataRate dataRate = Adxl345DataRate.Default;
        private Adxl345Range range = Adxl345Range.Default;

        #endregion

        #region Instance Management

        public Adxl345Connection(I2cDeviceConnection connection)
        {
            this.connection = connection;
            Initalize();
        }

        #endregion

        #region Properties

        public const int DefaultAddress = 0x53;

        public Adxl345DataRate DataRate
        {
            get { return dataRate; }
            set { dataRate = value; UpdateDataRate(); }
        }

        public Adxl345Range Range
        {
            get { return range; }
            set { range = value; UpdateRange(); }
        }

        #endregion

        #region Methods

        public Adxl345Data GetData()
        {
            var x = new UnitsNet.Acceleration(ToAcceleration(ReadValue(Interop.DataX0, Interop.DataX1)));
            var y = new UnitsNet.Acceleration(ToAcceleration(ReadValue(Interop.DataY0, Interop.DataY1)));
            var z = new UnitsNet.Acceleration(ToAcceleration(ReadValue(Interop.DataZ0, Interop.DataZ1)));

            return new Adxl345Data() { X = x, Y = y, Z = z };
        }

        #endregion

        #region Private Helpers

        private static class Interop
        {
            public const byte DeviceId      = 0x00;
            public const byte ThresholdTap  = 0x1D;
            public const byte XAxisOffset   = 0x1E;
            public const byte YAxisOffset   = 0x1F;
            public const byte ZAxisOffset   = 0x20;
            public const byte TapDuration   = 0x21;
            public const byte TapLatency    = 0x22;
            public const byte TapWindow     = 0x23;
            public const byte ActivityThreshold = 0x24;
            public const byte InactivityThreshold = 0x25;
            public const byte InactivityTime = 0x26;
            public const byte AxisInactivityDetectionControl = 0x27;
            public const byte FreefallThreshold = 0x28;
            public const byte FreefallTime = 0x29;
            public const byte AxisTapControl = 0x2A;
            public const byte TapSource = 0x2B;
            public const byte DataRateAndModeControl = 0x2C;
            public const byte PowerControl = 0x2D;
            public const byte InterruptControl = 0x2E;
            public const byte InterruptMap = 0x2F;
            public const byte InterruptSource = 0x30;
            public const byte DataFormatControl = 0x31;
            public const byte DataX0 = 0x32;
            public const byte DataX1 = 0x33;
            public const byte DataY0 = 0x34;
            public const byte DataY1 = 0x35;
            public const byte DataZ0 = 0x36;
            public const byte DataZ1 = 0x37;
            public const byte FifoControl = 0x38;
            public const byte FifoStatis = 0x39;

        }

        private double ToAcceleration(int rawValue)
        {
            const double MG2GMultiplier = 0.004;
            const double Gravity = 9.80665;

            return rawValue * MG2GMultiplier * Gravity;
        }

        private void Initalize()
        {
            if(ReadByte(Interop.DeviceId) != 0xE5)
            {
                throw new InvalidOperationException("Device is not a ADXL345 Accelerometer");
            }

            WriteByte(Interop.PowerControl, 0x08);

            dataRate = (Adxl345DataRate)(ReadByte(Interop.DataRateAndModeControl) & 0x0F);
            range = (Adxl345Range)(ReadByte(Interop.DataFormatControl) & 0x03);
        }

        public void UpdateRange()
        {
            int format = ReadByte(Interop.DataFormatControl);

            format &= ~0x0F;
            format |= (byte)range;

            WriteByte(Interop.DataFormatControl, (byte)format);
        }

        public void UpdateDataRate()
        {
            WriteByte(Interop.DataRateAndModeControl, (byte)dataRate);
        }

        private byte ReadByte(byte address)
        {
            return ReadBytes(address, 1)[0];
        }

        private int ReadValue(byte address1, byte address2)
        {
            var byte1 = ReadByte(address1);
            var byte2 = ReadByte(address2);

            var g = byte1 | (byte2 << 8);
            if(g > 32767)
            {
                g -= 65536;
            }
            return g;
        }

        private byte[] ReadBytes(byte address, int byteCount)
        {
            connection.WriteByte(address);
            return connection.Read(byteCount);
        }

        private void WriteByte(byte address, byte data)
        {
            connection.Write(address, data);
        }

        #endregion

    }
}
