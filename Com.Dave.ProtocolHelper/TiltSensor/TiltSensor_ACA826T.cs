using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace TiltSensor
{
    /// <summary>
    /// ACA826T-全温补高精度数字输出型双轴倾角传感器
    /// </summary>
    public class TiltSensor_ACA826T
    {
        private byte _frameIdentifier = 0x68;
        private SerialPort _port = new SerialPort() { BaudRate = 9600, StopBits = StopBits.One, DataBits = 8, Parity = Parity.None, PortName = "COM3" };
        public TiltSensor_ACA826T()
        {
            var ss = SerialPort.GetPortNames();
            Port.Open();
            flag = true;
            Thread thread = new Thread(ThreadSendAndReceive) { IsBackground = true };
            thread.Start();
            // Port.DataReceived += _port_DataReceived;
        }
        public bool flag = false;
        private int _offset = 0;
        private int _portReceivedTimeOutCount = 0;

        public SerialPort Port
        {
            get
            {
                return _port;
            }

            set
            {
                _port = value;
            }
        }

        public void ThreadSendAndReceive()
        {
            while (flag)
            {
                Thread.Sleep(20);
                if (Port.BytesToRead > 0)
                {
                    if (Port.BytesToRead != _offset)
                    {
                        _offset = Port.BytesToRead;
                        _portReceivedTimeOutCount = 0;
                    }
                    else
                    {
                        _portReceivedTimeOutCount++;
                        if (_portReceivedTimeOutCount > 5)
                        {
                            _portReceivedTimeOutCount = 0;
                            byte[] buffers = new byte[_offset];
                            _port.Read(buffers, 0, _offset);
                            Console.WriteLine("\r\n{0:D2}s{1:D3}ms Data Received:", DateTime.Now.Second,DateTime.Now.Millisecond);
                            foreach (byte item in buffers)
                            {
                                Console.Write(item.ToString("X2")+" ");
                            }
                            Console.WriteLine("\r\nlength:"+buffers.Length);
                            _offset = 0;
                        }
                    }
                }
            }
        }
        private void _port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            Console.WriteLine("\r\nData Received:");
            Console.Write(indata);
        }
    }
}
