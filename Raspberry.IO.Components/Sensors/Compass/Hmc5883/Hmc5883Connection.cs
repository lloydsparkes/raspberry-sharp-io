using Raspberry.IO.InterIntegratedCircuit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raspberry.IO.Components.Sensors.Compass.Hmc5883
{
    public class Hmc5883Connection
    {
        #region Fields

        private readonly I2cDeviceConnection connection;

        private Hmc5883DataRate dataRate = Hmc5883DataRate.Default;
        private Hmc5883SampleRate sampleRate = Hmc5883SampleRate.Default;
        private Hmc5883Mode mode = Hmc5883Mode.Default;

        private Hmc5883MeasurementMode measurementMode = Hmc5883MeasurementMode.Default;

        private Hmc5883Gain gain = Hmc5883Gain.Default;

        #endregion

        #region Instance Management

        public Hmc5883Connection(I2cDeviceConnection connection)
        {
            this.connection = connection;
            Initalize();
        }

        #endregion

        #region Properties

        public const int DefaultAddress = 0x1E;

        public Hmc5883DataRate DataRate
        {
            get { return dataRate; }
            set { dataRate = value;  UpdateControlA(); }
        }

        public Hmc5883SampleRate SampleRate
        {
            get { return sampleRate; }
            set { sampleRate = value; UpdateControlA(); }
        }

        public Hmc5883Mode Mode
        {
            get { return mode; }
            set { mode = value; UpdateControlA(); }
        }

        public Hmc5883MeasurementMode MeasurementMode
        {
            get { return measurementMode; }
            set { measurementMode = value; UpdateMode(); }
        }

        public Hmc5883Gain Gain
        {
            get { return gain; }
            set { gain = value; UpdateControlB(); }
        }

        public double XOffset { get; set; }
        public double YOffset { get; set; }
        public double ZOffset { get; set; }

        #endregion

        #region Methods

        public Hmc5883Data GetData()
        {
            var x = ScaleForGain(ReadValue(Interop.XHigher, Interop.XLower));
            var y = ScaleForGain(ReadValue(Interop.YHigher, Interop.YLower));
            var z = ScaleForGain(ReadValue(Interop.ZHigher, Interop.ZLower));

            return new Hmc5883Data() { X = x, Y = y, Z = z };
        }

        #endregion

        #region Private Helpers

        private static class Interop
        {
            public const byte ControlA = 0x00;
            public const byte ControlB = 0x01;
            public const byte Mode = 0x02;
            public const byte XHigher = 0x03;
            public const byte XLower = 0x04;
            public const byte YHigher = 0x05;
            public const byte YLower = 0x06;
            public const byte ZHigher = 0x07;
            public const byte ZLower = 0x08;
            public const byte Status = 0x09;
            public const byte DeviceIdA = 0x10;
            public const byte DeviceIdB = 0x11;
            public const byte DeviceIdC = 0x12;

            public const byte A_SampleRateMask = 0x60;
            public const byte A_DataRateMask = 0x1C;
            public const byte A_ModeMask = 0x03;
            public const byte B_GainMask = 0xE0;
            public const byte ModeMask = 0x03;

        }

        private void Initalize()
        {
            /*if (ReadByte(Interop.DeviceIdA) != 0x48
                && ReadByte(Interop.DeviceIdB) != 0x34
                && ReadByte(Interop.DeviceIdC) != 0x33)
            {
                throw new InvalidOperationException(string.Format("Device is not a HMC5883 Compass. Address: {0:X2} Value: {1:X2} {2:X2} {3:X2}", connection.DeviceAddress, ReadByte(Interop.DeviceIdA), ReadByte(Interop.DeviceIdB), ReadByte(Interop.DeviceIdC)));
            }*/

            UpdateControlA();
            UpdateControlB();
            UpdateMode();
        }

        private double ScaleForGain(double rawValue)
        {
            switch (gain)
            {
                case Hmc5883Gain.GA0_88:
                    return rawValue * 0.73;
                case Hmc5883Gain.GA1_30:
                default:
                    return rawValue * 0.92;
                case Hmc5883Gain.GA1_90:
                    return rawValue * 1.22;
                case Hmc5883Gain.GA2_50:
                    return rawValue * 1.52;
                case Hmc5883Gain.GA4_00:
                    return rawValue * 2.27;
                case Hmc5883Gain.GA4_70:
                    return rawValue * 2.56;
                case Hmc5883Gain.GA5_60:
                    return rawValue * 3.03;
                case Hmc5883Gain.GA8_10:
                    return rawValue * 4.35;
            }
        }

        private void UpdateControlA()
        {
            var value = (byte)dataRate & (byte)sampleRate & (byte)mode;
            WriteByte(Interop.ControlA, (byte)value);
        }

        private void UpdateControlB()
        {
            var value = 0x00 & (byte)gain;
            WriteByte(Interop.ControlB, (byte)value);
        }

        private void UpdateMode()
        {
            var value = 0x00 & (byte)measurementMode;
            WriteByte(Interop.Mode, (byte)value);
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
            if (g > 32767)
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
