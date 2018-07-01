using Com.Dave.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using TiltSensor.Entity;
using static TiltSensor.Common.CommonEnum;

namespace TiltSensor
{
    /// <summary>
    /// ACA826T-全温补高精度数字输出型双轴倾角传感器
    /// </summary>
    public class TiltSensor_ACA826T : IDisposable
    {
        public Thread _thread;
        private byte[] _readAngleCommand;
        private List<int> _frameHeadIndexList;
        private byte _frameIdentifier = 0x68;
        private Stopwatch _stopwatch = new Stopwatch();
        /// <summary>
        /// 
        /// </summary>
        private SerialPortOperationState _portOperationState = SerialPortOperationState.None;
        /// <summary>
        /// 
        /// </summary>
        private SerialPortOperationResult _portOperatingResult = SerialPortOperationResult.None;
        private SerialPort _port = new SerialPort() { BaudRate = 9600, StopBits = StopBits.One, DataBits = 8, Parity = Parity.None };
        public TiltSensor_ACA826T(string portName)
        {
            _port.PortName = portName;
            _readAngleCommand = StrHexHelper.StringToHex("68 04 00 04 08");
            _thread = new Thread(ThreadSendAndReceive) { IsBackground = true };
        }
        public void StartCollectData()
        {
            flag = true;
            _portOperationState = SerialPortOperationState.Send;
            _thread.Start();
        }
        public void StopCollectData()
        {
            flag = false;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool flag = false;
        private int _offset = 0;
        private int _portReceivedTimeOutCount = 0;
        private int _previousSecond = 0;
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
            int second = DateTime.Now.Second;
            if (_previousSecond == second)
            {
                return;
            }
            _previousSecond = second;
            _portOperationState = SerialPortOperationState.Receive;
            _offset = 0;
            _portOperatingResult = SerialPortOperationResult.None;
            _port.Write(_readAngleCommand, 0, _readAngleCommand.Length);
            _stopwatch.Stop();
            _stopwatch.Reset();
            _stopwatch.Start();
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
                        if (!flag)
                        {
                            _offset = 0;
                            return;
                        }
                        _port.Read(buffers, 0, _offset);

                        CheckFrame(buffers);
                        if (_frameHeadIndexList.Count > 0)
                        {
                            foreach (int item in _frameHeadIndexList)
                            {
                                if (buffers[item + 3] == 0x84)
                                {
                                    GetTiltSensorModel(buffers, item);
                                    _portOperatingResult = SerialPortOperationResult.Succeessful;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            _portOperatingResult = SerialPortOperationResult.Failed;
                        }
                        _offset = 0;
                    }
                }
            }
        }
        private void CheckFrame(byte[] buffer)
        {
            _frameHeadIndexList = new List<int>();
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == _frameIdentifier && i + 1 < buffer.Length && i + buffer[i + 1] < buffer.Length)
                {
                    int CS = 0, temp = i + 1, length = buffer[temp];
                    for (; temp < length; temp++)
                    {
                        CS += buffer[temp];
                    }
                    if (buffer[length + i] == (byte)CS)
                    {
                        _frameHeadIndexList.Add(i);
                    }
                }
            }
        }
        private TiltSensorModel GetTiltSensorModel(byte[] buffer, int index)
        {
            TiltSensorModel sensor = new TiltSensorModel();
            List<StringBuilder> sbList = new List<StringBuilder>();
            for (int i = index + 4; i < buffer.Length; i = (i + 4))
            {
                StringBuilder sb = new StringBuilder();
                if ((buffer[i] & 0x10) == 0x10)
                {
                    sb.Append("-");
                }
                else
                {
                    sb.Append("+");
                }
                sb.Append((buffer[i] & 0x0F) + "");
                sb.Append(((buffer[i + 1] & 0xF0) >> 4) + "");
                sb.Append((buffer[i + 1] & 0x0F) + "");
                sb.Append(".");
                sb.Append(((buffer[i + 2] & 0xF0) >> 4) + "");

                sb.Append((buffer[i + 2] & 0x0F) + "");
                sb.Append(((buffer[i + 3] & 0xF0) >> 4) + "");

                sb.Append((buffer[i + 3] & 0x0F) + "");
                sbList.Add(sb);
                if (sbList.Count > 2)
                    break;
            }
            sensor.XAsixDataValue = double.Parse(sbList[0].ToString());
            sensor.YAsixDataValue = double.Parse(sbList[1].ToString());
            sensor.TemperatureDataValue = double.Parse(sbList[2].ToString());
            sensor.CollectTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine(sensor.ToString());
            return sensor;
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
                switch (_portOperationState)
                {
                    case SerialPortOperationState.None:
                        break;
                    case SerialPortOperationState.Send:
                        SerialPortSend();
                        break;
                    case SerialPortOperationState.Receive:
                        long time = _stopwatch.ElapsedMilliseconds;
                        if (time > 1000)
                        {
                            _stopwatch.Stop();
                            _stopwatch.Reset();
                            _portOperatingResult = SerialPortOperationResult.Failed;
                        }
                        switch (_portOperatingResult)
                        {
                            case SerialPortOperationResult.None:
                                SerialPortReceive();
                                break;
                            case SerialPortOperationResult.Succeessful:
                                _portOperationState = SerialPortOperationState.Send;
                                break;
                            case SerialPortOperationResult.Failed:
                                _portOperationState = SerialPortOperationState.Send;
                                break;
                        }

                        break;
                }

            }
            flag = false;
            if (_port.IsOpen)
            {
                Port.Close();
            }
        }

        public void Dispose()
        {
            flag = false;
            try
            {
                _port.Dispose();
            }
            catch (Exception ex)
            {

            }


        }
    }
}
