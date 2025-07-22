using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwachhCityManagementTool.Models.SC
{
    public class MACAddressGenerator
    {
        public static string GenerateMACAddress()
        {
            var sBuilder = new StringBuilder();
            var r = new Random();
            int number;
            byte b;
            for (int i = 0; i < 6; i++)
            {
                number = r.Next(0, 255);
                b = Convert.ToByte(number);
                if (i == 0)
                {
                    b = setBit(b, 6); //--> set locally administered
                    b = unsetBit(b, 7); // --> set unicast 
                }
                sBuilder.Append(number.ToString("X2"));
                if (i != 5)
                {
                    sBuilder.Append(":");
                }


            }
            return sBuilder.ToString().ToUpper();
        }

        private static byte setBit(byte b, int BitNumber)
        {
            if (BitNumber < 8 && BitNumber > -1)
            {
                return (byte)(b | (byte)(0x01 << BitNumber));
            }
            else
            {
                throw new InvalidOperationException(
                "Der Wert für BitNumber " + BitNumber.ToString() + " war nicht im zulässigen Bereich! (BitNumber = (min)0 - (max)7)");
            }
        }

        private static byte unsetBit(byte b, int BitNumber)
        {
            if (BitNumber < 8 && BitNumber > -1)
            {
                return (byte)(b | (byte)(0x00 << BitNumber));
            }
            else
            {
                throw new InvalidOperationException(
                "Der Wert für BitNumber " + BitNumber.ToString() + " war nicht im zulässigen Bereich! (BitNumber = (min)0 - (max)7)");
            }
        }
    }
}
