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
            this.Dispatcher.BeginInvoke(new Action(() => {
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
                    break;
                case "btnStopCollect":
                    break;
            }
        }

        private void btnCollect_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
