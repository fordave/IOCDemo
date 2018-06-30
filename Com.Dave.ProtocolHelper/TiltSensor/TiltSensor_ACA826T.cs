using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using static TiltSensor.Common.CommonEnum;

namespace TiltSensor
{
    /// <summary>
    /// ACA826T-全温补高精度数字输出型双轴倾角传感器
    /// </summary>
    public class TiltSensor_ACA826T:IDisposable
    {
        private byte _frameIdentifier = 0x68;
        /// <summary>
        /// 
        /// </summary>
        private SerialPortOperationState _portOperationState = SerialPortOperationState.None;
        /// <summary>
        /// 
        /// </summary>
        private SerialPortOperationResult _portOperatingResult = SerialPortOperationResult.None;
        private SerialPort _port = new SerialPort() { BaudRate = 9600, StopBits = StopBits.One, DataBits = 8, Parity = Parity.None, PortName = "COM3" };
        public TiltSensor_ACA826T(string portName)
        {
            _port.PortName = portName;      
                     
        }
        public void StartCollectData()
        {
            flag = true;
            Thread thread = new Thread(ThreadSendAndReceive) { IsBackground = true };
            thread.Start();
        }
        /// <summary>
        /// 
        /// </summary>
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
        private void SerialPortSend()
        {

        }
        private void SerialPortReceive()
        {
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
                        Console.WriteLine("\r\n{0:D2}s{1:D3}ms Data Received:", DateTime.Now.Second, DateTime.Now.Millisecond);
                        foreach (byte item in buffers)
                        {
                            Console.Write(item.ToString("X2") + " ");
                        }
                        Console.WriteLine("\r\nlength:" + buffers.Length);
                        _offset = 0;
                    }
                }
            }
        }
        /// <summary>
        /// 串口发送接收
        /// </summary>
        public void ThreadSendAndReceive()
        {
            if (!_port.IsOpen)
            {
                Port.Open();
            }           
            while (flag)
            {
                Thread.Sleep(10);

               
            }
            flag = false;
            if (_port.IsOpen)
            {
                Port.Close();
            }
        }

        void Dispose()
        {
            flag = false;
            _port.Dispose();

        }
    }
}
