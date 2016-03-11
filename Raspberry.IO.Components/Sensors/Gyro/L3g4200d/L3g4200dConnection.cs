using Raspberry.IO.InterIntegratedCircuit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raspberry.IO.Components.Sensors.Gyro.L3g4200d
{
    public class L3g4200dConnection
    {
        #region Fields

        private readonly I2cDeviceConnection connection;
        private L3g4200dScale scale;

        #endregion

        #region Instance Management

        public L3g4200dConnection(I2cDeviceConnection connection)
        {
            this.connection = connection;
            Initalize();
        }

        #endregion

        #region Properties

        public const int DefaultAddress = 0x68;

        public L3g4200dScale Scale
        {
            get { return scale; }
            set { scale = value; UpdateScale(); }
        }

        #endregion

        #region Methods

        public L3g4200dData GetData()
        {
            var x = ScaleValue(ReadValue(Interop.XHigher, Interop.XLower));
            var y = ScaleValue(ReadValue(Interop.YHigher, Interop.YLower));
            var z = ScaleValue(ReadValue(Interop.ZHigher, Interop.ZLower));

            return new L3g4200dData() { X = x, Y = y, Z = z };
        }

        #endregion

        #region Private Helpers

        private static class Interop
        {
            public const byte DeviceId = 0x0F;
            public const byte Control1 = 0x20;
            public const byte Control2 = 0x21;
            public const byte Control3 = 0x22;
            public const byte Control4 = 0x23;
            public const byte Control5 = 0x24;
            public const byte Reference = 0x25;
            public const byte Temperature = 0x26;
            public const byte Status = 0x27;
            public const byte XLower = 0x28;
            public const byte XHigher = 0x29;
            public const byte YLower = 0x2A;
            public const byte YHigher = 0x2B;
            public const byte ZLower = 0x2C;
            public const byte ZHigher = 0x2D;
            public const byte FifoControl = 0x2E;
            public const byte FifoSource = 0x2F;
            public const byte InterruptConfig = 0x30;
            public const byte InterruptSource = 0x31;
            public const byte InterruptThresholdXHigher = 0x32;
            public const byte InterruptThresholdXLower = 0x33;
            public const byte InterruptThresholdYHigher = 0x34;
            public const byte InterruptThresholdYLower = 0x35;
            public const byte InterruptThresholdZHigher = 0x36;
            public const byte InterruptThresholdZLower = 0x37;
            public const byte InterruptDuration = 0x38;
        }

        private void Initalize()
        {
            if (ReadByte(Interop.DeviceId) != 0xD3)
            {
                throw new InvalidOperationException("Device is not a L3G4200D Gyro");
            }

            WriteByte(Interop.Control1, 0x0F);

            scale = (L3g4200dScale)(ReadByte(Interop.Control4));
        }

        private void UpdateScale()
        {
            WriteByte(Interop.Control4, (byte)scale);
        }

        private byte ReadByte(byte address)
        {
            return ReadBytes(address, 1)[0];
        }

        private double ScaleValue(int rawValue)
        {
            switch (scale)
            {
                case L3g4200dScale.S2000:
                    return ToRadians(rawValue * (70.00 / 1000));
                case L3g4200dScale.S500:
                    return ToRadians(rawValue * (17.50 / 1000));
                case L3g4200dScale.S250:
                default:
                    return ToRadians(rawValue * (8.75 / 1000));
            }
        }

        private double ToRadians(double degrees)
        {
            return (Math.PI / 180) * degrees;
        }

        private int ReadValue(byte higher, byte lower)
        {
            var higherByte = ReadByte(higher);
            var lowerByte = ReadByte(lower);

            var g = (higherByte << 8) | lowerByte;
            
            if((g & (1 << 15)) == 1)
            {
                return g | ~65535;
            }

            return g & 65535;
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
