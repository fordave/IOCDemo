using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfTest.View
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        private List<Demo> _list = new List<Demo>();
        public Window1()
        {
            Random random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                Demo demo = new Demo() { Xpath = 100*random.NextDouble(), Ypath = 100*random.NextDouble() };
                List.Add(demo);
            }
           
            InitializeComponent();
            scatter1.PointsSource = List;
        }

        public List<Demo> List
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
