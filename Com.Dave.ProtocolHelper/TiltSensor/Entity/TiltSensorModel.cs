using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiltSensor.Entity
{
    public class TiltSensorModel : NotifyPropertyChanged
    {
        private double _xAsixDataValue = 0;
        private double _yAsixDataValue = 0;
        private double _temperatureDataValue = 0;
        private string _collectTime = string.Empty;

        public double XAsixDataValue
        {
            get
            {
                return _xAsixDataValue;
            }

            set
            {
                _xAsixDataValue = value;
                OnPropertyChanged("XAsixDataValue");
            }
        }

        public double YAsixDataValue
        {
            get
            {
                return _yAsixDataValue;
            }

            set
            {
                _yAsixDataValue = value;
                OnPropertyChanged("YAsixDataValue");
            }
        }

        public double TemperatureDataValue
        {
            get
            {
                return _temperatureDataValue;
            }

            set
            {
                _temperatureDataValue = value;
                OnPropertyChanged("TemperatureDataValue");
            }
        }

        public string CollectTime
        {
            get
            {
                return _collectTime;
            }

            set
            {
                _collectTime = value;
            }
        }
        private Random random;
        public void HandleData()
        {
            random = new Random(DateTime.Now.Millisecond);
            _xAsixDataValue += random.NextDouble() * 0.02;
            _yAsixDataValue += random.NextDouble() * 0.02;
        }
        public override string ToString()
        {
            return string.Format("x轴：{0:000.0000}\tY轴：{1:000.0000}\t温度：{2:000.0000}\t采集时间：{3}", 
                _xAsixDataValue, _yAsixDataValue, _temperatureDataValue, _collectTime);
        }
    }
}
