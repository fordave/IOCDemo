using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfTest.View
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        private ObservableCollection<Demo> _list = new ObservableCollection<Demo>();
        public Window1()
        {
            random = new Random();
            //for (int i = 0; i < 1000; i++)
            //{
            //    Demo demo = new Demo() { Xpath = 100 * random.NextDouble(), Ypath = 100 * random.NextDouble() };
            //    List.Add(demo);
            //}

            InitializeComponent();
            scatter1.PointsSource = List;
            _timer = new DispatcherTimer() { Interval = new TimeSpan(0,0,4) };
            _timer.Tick += _timer_Tick; ;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
           //this.Dispatcher.BeginInvoke(new Action(() =>
           // {
           //     for (int i = 0; i < 100; i++)
           //     {

           //         Demo demo = new Demo() { Xpath = 100 * random.NextDouble(), Ypath = 100 * random.NextDouble() };
           //         List.Add(demo);
           //     }
           // }), null);
            for (int i = 0; i < 100; i++)
            {

                Demo demo = new Demo() { Xpath = 100 * random.NextDouble(), Ypath = 100 * random.NextDouble() };
                List.Add(demo);
            }
        }

        private Random random;
       

        private DispatcherTimer _timer;

        public ObservableCollection<Demo> List
        {
            get
            {
                return _list;
            }

            set
            {
                _list = value;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _timer.Start();
        }
    }

    public class Demo
    {
        private double _xpath;
        private double _ypath;

        public double Xpath
        {
            get
            {
                return _xpath;
            }

            set
            {
                _xpath = value;
            }
        }

        public double Ypath
        {
            get
            {
                return _ypath;
            }

            set
            {
                _ypath = value;
            }
        }
    }
}
