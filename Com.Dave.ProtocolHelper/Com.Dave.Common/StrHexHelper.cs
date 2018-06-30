using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Dave.Common
{
    public class StrHexHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffers"></param>
        /// <returns></returns>
        public static string HexToString(byte[] buffers)
        {
            StringBuilder sb = new StringBuilder();
            if (buffers != null)
            {
                foreach (byte child in buffers)
                {
                    sb.Append(child.ToString("X2") + " ");
                }
            }
            return sb.ToString().Trim();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public static byte[] StringToHex(string temp)
        {
            if (!string.IsNullOrEmpty(temp))
            {
                string[] strArray = temp.Split(' ');
                byte[] byteArray = new byte[strArray.Length];
                for (int i = 0; i < strArray.Length; i++)
                {
                    byteArray[i] = Convert.ToByte(strArray[i], 16);
                }
                return byteArray;
            }
            return null;
        }
    }
}
