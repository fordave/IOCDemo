using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiltSensor.Common
{
    public class CommonEnum
    {
        public enum SerialPortOperationState
        {
            None,
            Send,
            Receive
        }
        public enum SerialPortOperationResult
        {
            None,
            Succeessful,
            Failed
        }
    }
}
