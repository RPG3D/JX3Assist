using System;
using System.Collections.Generic;
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
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Media;
using System.Threading;

namespace JxqyWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MyApp app = new MyApp();
        public Update upd = new Update();

        public MainWindow()
        {
            InitializeComponent();
            this.Title = app.windowTitle;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            app.Start();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            app.Stop();
        }

        private void btnSetTick_Click(object sender, RoutedEventArgs e)
        {
            int time = 50;
            if(int.TryParse(txtInTime.Text, out time))
            {
                if(time < 20 || time > 9999)
                {
                    MessageBox.Show("TickTime Should Between 20 and 9999");
                }
                else
                {
                    app.SetTickTime(time);
                    MessageBox.Show("Successful");
                } 
            }
            else
            {
                MessageBox.Show("TickTime Should Between 20 and 9999");
            }
            txtInTime.Text = time.ToString();
        }

        protected override void OnClosed(EventArgs e)
        {
            System.Environment.Exit(0);
            base.OnClosed(e);
        }

        private void btnCheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            upd.CheckUpdate();
        }
    }
}
