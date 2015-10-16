using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public class ChecksumHelper
    {
        public static int Checksum(string literal)
        {
            var hash = 0;
            var length = literal.Length;
            if (length == 0)
                return hash;

            for (var i = 0; i < length; i++)
            {
                hash = ((hash << 5) - hash) + (int)(Convert.ToChar(literal.Substring(i, 1)));
                hash = Convert.ToInt32(hash);
            }

            return hash;
        }

        public static string StringUtilsChecksum(string dataToCalculate)
        {
            var bytesToCalculate = Encoding.ASCII.GetBytes(dataToCalculate);
            int checksum = 0;

            foreach (byte chData in bytesToCalculate)
            {
                checksum += chData;
            }
            checksum &= 0xff;

            return checksum.ToString("X2");
        }
    }
}
