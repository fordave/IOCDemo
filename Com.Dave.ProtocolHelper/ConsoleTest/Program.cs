using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TiltSensor;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TiltSensor_ACA826T tilt = new TiltSensor_ACA826T("COM1");
            tilt.StartCollectData();
            Console.Read();
            tilt.StopCollectData();
            Thread.Sleep(100);
            tilt.Dispose();
         
        }
    }
}
