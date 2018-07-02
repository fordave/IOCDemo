using Com.Dave.DAL.DBHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using TiltSensor;
using TiltSensor.Entity;

namespace WpfTest.ViewModel
{
    public class TiltSensorViewModel : NotifyPropertyChanged, IDisposable
    {
        public TiltSensorViewModel()
        {
            TiltSensor = new TiltSensor_ACA826T(_datasource);
            TiltSensor.UpdateEvent += _tiltSensor_UpdateEvent;
        }

        private void _tiltSensor_UpdateEvent(object sender, UpdateEventArgs e)
        {

        }

        private ObservableCollection<TiltSensorModel> _tiltSensorModelCollection = new ObservableCollection<TiltSensorModel>();

        public ObservableCollection<TiltSensorModel> TiltSensorModelCollection
        {
            get
            {
                return _tiltSensorModelCollection;
            }

            set
            {
                _tiltSensorModelCollection = value;
            }
        }

        public TiltSensor_ACA826T TiltSensor
        {
            get
            {
                return _tiltSensor;
            }

            set
            {
                _tiltSensor = value;
            }
        }

        private string _datasource = @"data source =.\DBFile\demo.db;Version=3";
        private TiltSensor_ACA826T _tiltSensor;
        public void CollectData(string portName)
        {
            TiltSensor.StartCollectData(portName);
        }
        public void StopCollectData()
        {
            if (TiltSensor != null)
            {
                TiltSensor.StopCollectData();
            }
        }
        string _sqlSelectTop100 = @"SELECT
	                                    xasixdatavalue,
	                                    yasixdatavalue 
                                    FROM
	                                    tiltsensor 	                                   
                                    WHERE
	                                    time BETWEEN '";
        public DataTable GetDataTable(string startTime, string endTime)
        {
           // string sql = _sqlSelectTop100 + startTime + "' and '" + endTime + "';";
          string sql = _sqlSelectTop100 + startTime + "' and '" + endTime + "' LIMIT 100;";
            var t1 = DateTime.Now;
            DataSet set = SQLiteHelper.ExecuteDataSet(_datasource, sql, null);
            var t2 = DateTime.Now;
            Console.WriteLine("time cost :{0};", t2 - t1);
            var tt = set.Tables[0];
            Console.WriteLine("table row :{0}", tt.Rows.Count);
            return tt;

        }
        public void Dispose()
        {
            if (TiltSensor != null)
                TiltSensor.Dispose();
        }
    }
}
