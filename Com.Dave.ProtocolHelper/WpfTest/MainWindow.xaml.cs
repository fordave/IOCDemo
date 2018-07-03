using Com.Dave.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TiltSensor.Entity;
using WpfTest.ViewModel;

namespace WpfTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            _tiltSensorViewModel = new TiltSensorViewModel();
            _tiltSensorViewModel.TiltSensor.UpdateEvent += TiltSensor_UpdateEvent;
            InitializeComponent();
            cmbSerialPort.ItemsSource = SerialPort.GetPortNames();
            datagrid1.ItemsSource = TiltSensorModelCollection;
           
        }

        private void TiltSensor_UpdateEvent(object sender, TiltSensor.UpdateEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                for (int i = 0; i < e.TiltSensorModelCollection.Count; i++)
                {
                    _tiltSensorModelCollection.Add(e.TiltSensorModelCollection[i]);
                }
            }));

        }
        private ObservableCollection<TiltSensorModel> _tiltSensorModelCollection = new ObservableCollection<TiltSensorModel>();

        private TiltSensorViewModel _tiltSensorViewModel;

        public TiltSensorViewModel TiltSensorViewModel
        {
            get
            {
                return _tiltSensorViewModel;
            }

            set
            {
                _tiltSensorViewModel = value;
            }
        }

        public ObservableCollection<TiltSensorModel> TiltSensorModelCollection
        {
            get
            {
                return TiltSensorModelCollection1;
            }

            set
            {
                TiltSensorModelCollection1 = value;
            }
        }

        private PointCollection _pointList = new PointCollection();
        public ObservableCollection<TiltSensorModel> TiltSensorModelCollection1
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

        public PointCollection PointList
        {
            get
            {
                return _pointList;
            }

            set
            {
                _pointList = value;
            }
        }

        private bool _isEnableButton = false;
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null)
                return;
            switch (btn.Name)
            {
                case "btnCollect":
                    if (cmbSerialPort.SelectedIndex == -1)
                    {
                        MessageBox.Show("First set serialPort!");
                        return;
                    }
                    TiltSensorViewModel.CollectData(cmbSerialPort.SelectedItem as string);
                    btnCollect.IsEnabled = _isEnableButton;
                    btnStopCollect.IsEnabled = !_isEnableButton;
                    break;
                case "btnStopCollect":
                    btnCollect.IsEnabled = !_isEnableButton;
                    btnStopCollect.IsEnabled = _isEnableButton;
                    TiltSensorViewModel.StopCollectData();
                    break;
                case "btnSelect":
                    if (picker1.Value == null || picker2.Value == null)
                    {
                        MessageBox.Show("select timespan fisrt");
                        return;
                    }
                    var tt = TiltSensorViewModel.GetDataTable(((DateTime)picker1.Value).ToString("yyyy-MM-dd HH:mm:ss"), ((DateTime)picker2.Value).ToString("yyyy-MM-dd HH:mm:ss"));
                    if (tt != null)
                    {
                        var ttt = DateTime.Now;
                        var t1 = DataTableListHelper<TiltSensorModel>.DataTableToList(tt);
                        var ttt2 = DateTime.Now;
                        Console.WriteLine("convert cost time: {0}", ttt2 - ttt);
                        PointList.Clear();
                        if (t1 != null)
                        {
                            //this.Dispatcher.Invoke(new Action<List<TiltSensorModel>>(args => {
                            //    for (int i = 0; i < t1.Count; i++)
                            //    {
                            //        t1[i].HandleData();
                            //        TiltSensorModelList.Add(t1[i]);
                            //    }
                            //    var ttt3 = DateTime.Now;
                            //    Console.WriteLine("fill collection cost time: {0}", ttt3 - ttt2);
                            //}), DispatcherPriority.SystemIdle, t1);


                            for (int i = 0; i < t1.Count; i++)
                            {
                                t1[i].HandleData();
                                Point point = new Point() { X = t1[i].XAsixDataValue, Y = t1[i].YAsixDataValue };
                                PointList.Add(point);
                                //TiltSensorModelList.Add(t1[i]);
                            }
                            scatter1.ScatterPoints = PointList;
                            scatter1.Refresh();
                            var ttt3 = DateTime.Now;
                            Console.WriteLine("fill collection cost time: {0}", ttt3 - ttt2);
                        }
                    }
                    break;
            }
        }
        public ObservableCollection<TiltSensorModel> TiltSensorModelList = new ObservableCollection<TiltSensorModel>();
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TiltSensorViewModel.Dispose();
        }
    }
}
